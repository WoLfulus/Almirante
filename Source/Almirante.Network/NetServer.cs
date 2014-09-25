using Almirante.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Almirante.Network
{
    /// <summary>
    /// GameServer class.
    /// </summary>
    public class NetServer<T>
        where T : NetConnection, new()
    {
        /// <summary>
        /// Server capacity.
        /// </summary>
        private int capacity;

        /// <summary>
        /// Server listener.
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// Server protocol handler.
        /// </summary>
        protected NetServerProtocol<T> Protocol
        {
            get;
            private set;
        }

        /// <summary>
        /// Client indexes.
        /// </summary>
        private Queue<int> ids;

        /// <summary>
        /// Connection list.
        /// </summary>
        private Dictionary<int, T> connections;

        /// <summary>
        /// List of connections
        /// </summary>
        public T[] Connections
        {
            get
            {
                lock (this)
                {
                    return this.connections.Values.ToArray();
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="protocol">Protocol message handler</param>
        /// <param name="capacity">Server capacity</param>
        public NetServer(int capacity)
        {
            this.Protocol = new NetServerProtocol<T>();
            this.Protocol.Server = this;
            this.capacity = capacity;
            this.connections = new Dictionary<int, T>(capacity);
            this.ids = new Queue<int>(capacity);
            for(int i = 0; i < capacity; i++)
            {
                this.ids.Enqueue(i);
            }
        }

        /// <summary>
        /// Starts the server on the specified port.
        /// </summary>
        /// <param name="port"></param>
        public void Start(int port)
        {
            try
            {
                this.listener = new TcpListener(IPAddress.Any, port);
                this.listener.Start();
                this.OnStart();
                this.Accept();
            }
            catch (Exception ex)
            {
                this.OnError(ex);
            }
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        public void Stop()
        {
            if (this.listener == null)
            {
                return;
            }
            this.listener.Stop();
            this.listener = null;
            this.OnStop();
        }

        /// <summary>
        /// Accepts a socket.
        /// </summary>
        private void Accept()
        {
            this.listener.BeginAcceptSocket(this.AcceptCallback, null);
        }

        /// <summary>
        /// Accept socket callback.
        /// </summary>
        /// <param name="result">Asynchronous result.</param>
        private void AcceptCallback(IAsyncResult result)
        {
            try
            {
                Socket socket = this.listener.EndAcceptSocket(result);
                lock (this)
                {
                    if (this.ids.Count == 0)
                    {
                        socket.Close(); // Server is full
                    }
                    else
                    {
                        int id = this.ids.Dequeue();
                        
                        var conn = new T();
                        conn.Disconnected += ConnectionDisconnected;
                        conn.Data += ConnectionMessage;
                        conn.Initialize(id, socket);

                        this.connections[id] = conn;

                        this.OnConnect(conn);
                    }
                }

                this.Accept();
            }
            catch (NullReferenceException)
            {
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception e)
            {
                this.OnError(e);
            }
        }

        /// <summary>
        /// Start
        /// </summary>
        /// <param name="ex"></param>
        protected virtual void OnStart()
        {
        }

        /// <summary>
        /// Start
        /// </summary>
        /// <param name="ex"></param>
        protected virtual void OnStop()
        {
        }

        /// <summary>
        /// Error callback
        /// </summary>
        /// <param name="ex"></param>
        protected virtual void OnError(Exception ex)
        {
        }

        /// <summary>
        /// Connected callback
        /// </summary>
        /// <param name="connection"></param>
        protected virtual void OnConnect(T connection)
        {
        }

        /// <summary>
        /// Disconnected callback
        /// </summary>
        /// <param name="connection"></param>
        protected virtual void OnDisconnect(T connection)
        {
        }

        /// <summary>
        /// Message handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectionMessage(object sender, DataEventArgs e)
        {
            var conn = sender as T;
            if (conn != null)
            {
                lock (this)
                {
                    this.Protocol.Handle(conn, e.Id, e.Buffer);
                }
            }
        }

        /// <summary>
        /// Connection lost.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectionDisconnected(object sender, DisconnectedEventArgs e)
        {
            var conn = sender as NetConnection;
            if (conn != null)
            {
                lock (this)
                {
                    if (this.connections.Remove(conn.Id))
                    {
                        this.OnDisconnect(conn as T);
                    }
                }
            }
        }
    }
}
