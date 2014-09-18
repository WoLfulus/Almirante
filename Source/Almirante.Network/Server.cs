using Almirante.Network;
using System;
using System.Collections.Generic;
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
    public class Server<T>
        where T : Connection, new()
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
        private Protocol<T> protocol;

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
        /// Events
        /// </summary>
        public event EventHandler Started;
        public event EventHandler Stopped;
        public event EventHandler<ErrorEventArgs> Error;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="protocol">Protocol message handler</param>
        /// <param name="capacity">Server capacity</param>
        public Server(Protocol<T> protocol, int capacity)
        {
            this.protocol = protocol;
            this.protocol.Server = this;
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
                if (this.Started != null)
                {
                    this.Started(this, new EventArgs());
                }
                this.Accept();
            }
            catch (Exception ex)
            {
                if (this.Error != null)
                {
                    this.Error(this, new ErrorEventArgs() 
                    {
                        Error = ex 
                    });
                }
            }
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        public void Stop()
        {
            this.listener.Stop();
            this.listener = null;
            if (this.Stopped != null)
            {
                this.Stopped(this, new EventArgs());
            }
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
                        conn.Disconnected += OnDisconnect;
                        conn.Message += OnMessage;
                        conn.Initialize(id, socket);

                        this.connections[id] = conn;
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
                Console.WriteLine("[ERROR] " + e.Message);
            }
        }

        /// <summary>
        /// Connection lost.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMessage(object sender, MessageEventArgs e)
        {
            var conn = sender as T;
            if (conn != null)
            {
                lock (this)
                {
                    this.protocol.Handle(conn, e.Id, e.Buffer);
                }
            }
        }

        /// <summary>
        /// Connection lost.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDisconnect(object sender, DisconnectedEventArgs e)
        {
            var conn = sender as Connection;
            if (conn != null)
            {
                lock (this)
                {
                    this.connections.Remove(conn.Id);
                }
            }
        }
    }
}
