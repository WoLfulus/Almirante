using System;
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
        /// Constructor
        /// </summary>
        public NetClient()
        {
            this.Protocol = new NetClientProtocol();
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
                this.OnConnect(true);
            }
            catch (Exception ex)
            {
                this.OnError(ex);
                this.OnConnect(false);
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
                        this.OnDisconnect();
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

        /// <summary>
        /// Callback.
        /// </summary>
        /// <param name="result"></param>
        private void SendCallback(IAsyncResult result)
        {
            byte[] buffer = result.AsyncState as byte[];
            int bytes = this.socket.EndSend(result);
            if (bytes != buffer.Length)
            {
                Debug.WriteLine("Send callback size differs from buffer length (" + bytes + " != " + buffer.Length + ")");
            }
        }

        /// <summary>
        /// Receive request.
        /// </summary>
        private void Receive()
        {
            this.socket.BeginReceive(this.buffer, this.bufferOffset, this.buffer.Length - this.bufferOffset, SocketFlags.None, this.ReceiveCallback, null);
        }

        /// <summary>
        /// Receive callback
        /// </summary>
        /// <param name="result"></param>
        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                SocketError error = SocketError.Success;
                int bytes = this.socket.EndReceive(result, out error);
                if (error != SocketError.Success)
                {
                    if (error != SocketError.ConnectionReset)
                    {
                        this.OnError(new Exception("EndReceive failed with error " + error.ToString()));
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

                                    if (size < this.bufferOffset)
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
                                        this.OnError(ex);
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
                this.OnError(e);
                this.Disconnect();
            }
        }

        protected virtual void OnConnect(bool success)
        {
        }

        protected virtual void OnDisconnect()
        {
        }

        protected virtual void OnError(Exception ex)
        {
            Debug.WriteLine("[Client Error] " + ex.Message);
        }
    }
}
