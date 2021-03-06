﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Almirante.Network
{
    /// <summary>
    /// Client
    /// </summary>
    public class NetClient
    {
        /// <summary>
        /// Data
        /// </summary>
        private class EventData
        {
            public enum EventDataType
            {
                Error,
                Connect,
                Disconnect
            }

            public EventDataType Type
            {
                get;
                set;
            }

            public object Data
            {
                get;
                set;
            }
        }

        /// <summary>
        /// Socket.
        /// </summary>
        private Socket socket;

        /// <summary>
        /// Protocol handler.
        /// </summary>
        public NetClientProtocol Protocol
        {
            get;
            private set;
        }

        /// <summary>
        /// Buffer.
        /// </summary>
        private byte[] buffer = new byte[4096];

        /// <summary>
        /// Buffer offset.
        /// </summary>
        private int bufferOffset = 0;

        /// <summary>
        /// Events
        /// </summary>
        private Queue<EventData> events;

        /// <summary>
        /// Constructor
        /// </summary>
        public NetClient()
        {
            this.Protocol = new NetClientProtocol();
            this.events = new Queue<EventData>();
        }

        /// <summary>
        /// Updates the calls
        /// </summary>
        public void Update()
        {
            lock (this)
            {
                this.Protocol.Process();
                while (this.events.Count > 0)
                {
                    var q = this.events.Dequeue();
                    if (q != null)
                    {
                        switch (q.Type)
                        {
                            case EventData.EventDataType.Connect:
                                this.OnConnect((bool)q.Data);
                                break;
                            case EventData.EventDataType.Disconnect:
                                this.OnDisconnect();
                                break;
                            case EventData.EventDataType.Error:
                                this.OnError((Exception) q.Data);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Connects the socket.
        /// </summary>
        /// <param name="host">Hostname</param>
        /// <param name="port">Port</param>
        public void Connect(string host, int port)
        {
            if (this.socket != null)
            {
                this.Disconnect();
            }

            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.socket.BeginConnect(host, port, this.ConnectCallback, null);
        }

        /// <summary>
        /// Connection callback.
        /// </summary>
        /// <param name="result"></param>
        private void ConnectCallback(IAsyncResult result)
        {
            try
            {
                this.socket.EndConnect(result);
                this.Receive();
                this.OnConnectCall(true);
            }
            catch (Exception ex)
            {
                this.OnErrorCall(ex);
                this.OnConnectCall(false);
            }
        }

        /// <summary>
        /// Disconnects the socket.
        /// </summary>
        public void Disconnect()
        {
            lock (this)
            {
                if (this.socket != null)
                {
                    try
                    {
                        this.socket.Shutdown(SocketShutdown.Both);
                        this.socket.Close(100);
                        this.socket = null;
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        this.OnDisconnectCall();
                    }
                }
            }
        }

        /// <summary>
        /// Sends a packet to this connection.
        /// </summary>
        /// <param name="packet">Packet instance.</param>
        public void Send<P>(P packet)
            where P : Packet
        {
            lock (this)
            {
                if (this.socket == null || !this.socket.Connected)
                {
                    throw new Exception("You must connect NetClient first.");
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8))
                    {
                        byte[] buffer = packet.Write();
                        writer.Write(buffer.Length + 8);
                        writer.Write(packet.Id);
                        writer.Write(buffer);
                        writer.Flush();
                        byte[] data = stream.ToArray();
                        try
                        {
                            lock (this)
                            {
                                this.socket.BeginSend(data, 0, data.Length, SocketFlags.None, this.SendCallback, data);
                            }
                        }
                        catch (System.Exception)
                        {
                            this.Disconnect();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Callback.
        /// </summary>
        /// <param name="result"></param>
        private void SendCallback(IAsyncResult result)
        {
            try
            {
                byte[] buffer = result.AsyncState as byte[];
                int bytes = this.socket.EndSend(result);
                if (bytes != buffer.Length)
                {
                    Debug.WriteLine("Send callback size differs from buffer length (" + bytes + " != " + buffer.Length + ")");
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Receive request.
        /// </summary>
        private void Receive()
        {
            try
            {
                this.socket.BeginReceive(this.buffer, this.bufferOffset, this.buffer.Length - this.bufferOffset, SocketFlags.None, this.ReceiveCallback, null);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Receive callback
        /// </summary>
        /// <param name="result"></param>
        private void ReceiveCallback(IAsyncResult result)
        {
            lock (this)
            {
                try
                {
                    SocketError error = SocketError.Success;
                    int bytes = this.socket.EndReceive(result, out error);
                    if (error != SocketError.Success)
                    {
                        if (error != SocketError.ConnectionReset)
                        {
                            this.OnErrorCall(new Exception("EndReceive failed with error " + error.ToString()));
                        }
                        this.Disconnect();
                    }
                    else
                    {
                        if (bytes > 0)
                        {
                            this.bufferOffset += bytes;
                            while (true)
                            {
                                if (this.bufferOffset < 8)
                                {
                                    this.Receive();
                                    return;
                                }

                                using (MemoryStream stream = new MemoryStream(this.buffer, 0, this.bufferOffset, false))
                                {
                                    using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8))
                                    {
                                        int size = reader.ReadInt32();
                                        int id = reader.ReadInt32();

                                        if (size >= this.buffer.Length)
                                        {
                                            throw new Exception("Packet size is bigger than the receive buffer.");
                                        }

                                        if (size > this.bufferOffset)
                                        {
                                            this.Receive();
                                            return;
                                        }

                                        byte[] buffer = reader.ReadBytes(size - 8);

                                        try
                                        {
                                            this.Protocol.Handle(id, buffer);
                                        }
                                        catch (Exception ex)
                                        {
                                            this.OnErrorCall(ex);
                                        }

                                        // Messages copy to front
                                        for (int i = 0; i < this.bufferOffset - size; i++)
                                        {
                                            this.buffer[i] = this.buffer[size + i];
                                        }

                                        this.bufferOffset -= size;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.Disconnect();
                        }
                    }
                }
                catch (NullReferenceException)
                {
                }
                catch (Exception e)
                {
                    this.OnErrorCall(e);
                    this.Disconnect();
                }
            }
        }

        private void OnConnectCall(bool success)
        {
            lock (this)
            {
                this.events.Enqueue(new EventData()
                {
                    Type = EventData.EventDataType.Connect,
                    Data = success
                });
            }
        }

        private void OnDisconnectCall()
        {
            lock (this)
            {
                this.events.Enqueue(new EventData()
                {
                    Type = EventData.EventDataType.Disconnect
                });
            }
        }

        private void OnErrorCall(Exception ex)
        {
            lock (this)
            {
                this.events.Enqueue(new EventData()
                {
                    Type = EventData.EventDataType.Error,
                    Data = ex
                });
            }
        }

        /// <summary>
        /// Virtual
        /// </summary>
        /// <param name="success"></param>
        protected virtual void OnConnect(bool success)
        {
        }

        /// <summary>
        /// Virtual
        /// </summary>
        /// <param name="success"></param>
        protected virtual void OnDisconnect()
        {
        }

        /// <summary>
        /// Virtual
        /// </summary>
        /// <param name="success"></param>
        protected virtual void OnError(Exception ex)
        {
        }
    }
}
