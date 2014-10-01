using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Almirante.Network
{
    /// <summary>
    /// Connection class.
    /// </summary>
    public class NetConnection
    {
        /// <summary>
        /// Index.
        /// </summary>
        private int id;

        /// <summary>
        /// Connection id.
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Socket.
        /// </summary>
        private Socket socket;

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
        internal event EventHandler<DataEventArgs> Data;
        internal event EventHandler<DisconnectedEventArgs> Disconnected;

        /// <summary>
        /// Constructor
        /// </summary>
        public NetConnection()
        {
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="index">Connection id.</param>
        /// <param name="socket">Socket.</param>
        internal void Initialize(int id, Socket socket)
        {
            this.id = id;
            this.socket = socket;
            this.OnConnect();
            this.Receive();
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

                    this.OnDisconnect();
                    if (this.Disconnected != null)
                    {
                        this.Disconnected(this, new DisconnectedEventArgs());
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
                    catch (System.Exception ex)
                    {
                        this.OnError(ex);
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

                                    if (size > this.bufferOffset)
                                    {
                                        this.Receive();
                                        return;
                                    }

                                    byte[] buffer = reader.ReadBytes(size - 8);
                                    if (this.Data != null)
                                    {
                                        try
                                        {
                                            this.Data(this, new DataEventArgs()
                                            {
                                                Id = id,
                                                Buffer = buffer
                                            });
                                        }
                                        catch (Exception ex)
                                        {
                                            this.OnError(ex);
                                        }
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

        /// <summary>
        /// Error raise
        /// </summary>
        /// <param name="ex"></param>
        internal void Error(Exception ex)
        {
            this.OnError(ex);
        }

        /// <summary>
        /// Connected.
        /// </summary>
        protected virtual void OnConnect()
        {
        }

        /// <summary>
        /// Connected.
        /// </summary>
        protected virtual void OnDisconnect()
        {
        }

        /// <summary>
        /// Connected.
        /// </summary>
        protected virtual void OnError(Exception error)
        {
        }
    }
}
