using System;
using System.Collections.Generic;
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
    public class Connection
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
        internal event EventHandler<DisconnectedEventArgs> Disconnected;
        internal event EventHandler<ErrorEventArgs> Error;

        /// <summary>
        /// Constructor
        /// </summary>
        public Connection()
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
                    Console.WriteLine("RECEIVE ERROR: " + error.ToString());
                    this.Disconnect();
                }
                else
                {
                    if (bytes > 0)
                    {
                        this.bufferOffset += bytes;
                        if (this.bufferOffset <= 8)
                        {
                            this.Receive();
                            return;
                        }

                        MemoryStream stream = new MemoryStream(this.buffer, 0, this.bufferOffset, false);
                        BinaryReader reader = new BinaryReader(stream, Encoding.UTF8);
                        
                        int size = reader.ReadInt32();
                        int id = reader.ReadInt32();

                        byte[] message = reader.ReadBytes(size - 8);
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
                if (this.Error != null)
                {
                    this.Error(this, new ErrorEventArgs()
                    {
                        Error = e
                    });
                }
                this.Disconnect();
            }
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
    }
}
