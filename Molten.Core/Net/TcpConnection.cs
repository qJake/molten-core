using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Molten.Core.Net
{
    /// <summary>
    /// Encapsulates a single TCP connection in an easy to use wrapper class.
    /// </summary>
    /// <remarks>TODO: This needs to support various protocols / termination identifiers to know when to fire the DataReceived event. E.g. HTTP (checking length in header against length of body), line-by-line, byte sequence, etc.</remarks>
    public class TcpConnection
    {
        /// <summary>
        /// Occurs when incoming data arrives through the TCP connection. This event occurs on a separate thread.
        /// </summary>
        public event Action<string> DataReceived;

        /// <summary>
        /// Occurs when the TCP connection is established.
        /// </summary>
        public event Action Connected;
        
        /// <summary>
        /// Occurs when the TCP connection is terminated.
        /// </summary>
        public event Action Disconnected;

        /// <summary>
        /// Listener thread for accepting data.
        /// </summary>
        private Thread dataThread;

        /// <summary>
        /// Gets the host that this TCP connection is using.
        /// </summary>
        public string Host { get; private set; }

        /// <summary>
        /// Gets the port number that this TCP connection is using.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Gets whether or not the TCP connection is established.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return Client.Connected;
            }
        }

        /// <summary>
        /// Gets the underlying TCP client.
        /// </summary>
        public TcpClient Client { get; private set; }

        /// <summary>
        /// Initializes a new instance of the TcpConnection Class with the specified host and port number.
        /// </summary>
        /// <param name="host">The host to connect to. Can be an IP address or a domain name.</param>
        /// <param name="port">The port to communicate on.</param>
        public TcpConnection(string host, int port)
        {
            Host = host;
            Port = port;

            Client = new TcpClient();
            Client.Connect(Host, Port);
            if (Client.Connected)
            {
                dataThread = new Thread((ThreadStart)delegate()
                {
                    MessageListener();
                });
                dataThread.Start();
            }
            if (Connected != null)
            {
                Connected();
            }
        }

        /// <summary>
        /// Sends data through the TCP connection, optionally specifying the encoding to use.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <param name="encoding">Optionally, the encoding to use when sending data. If this is not set, it defaults to ASCII.</param>
        public void Send(string data, Encoding encoding = null)
        {
            Send(Encoding.ASCII.GetBytes(data));
        }

        /// <summary>
        /// Sends bytes through the TCP connection.
        /// </summary>
        /// <param name="data">The bytes to send.</param>
        public void Send(byte[] data)
        {
            BinaryWriter bw = new BinaryWriter(Client.GetStream());
            bw.Write(data);
            bw.Flush();
        }

        /// <summary>
        /// The message listener. This spins to listen for incoming data from the TCP connection.
        /// </summary>
        private void MessageListener()
        {
            try
            {
                var ns = Client.GetStream();
                StreamReader sr = new StreamReader(ns);
                string data = null;

                while (IsConnected)
                {
                    byte[] buffer = new byte[512];

                    if (ns.DataAvailable)
                    {
                        int count = ns.Read(buffer, 0, buffer.Length);
                        if (count > 0)
                        {
                            var usableBuffer = buffer.Take(count).ToArray();
                            data += Encoding.ASCII.GetString(usableBuffer);
                        }
                    }
                    else
                    {
                        if (data != null)
                        {
                            DataReceived(data);
                            data = null;
                        }
                    }
                }
                Disconnect();
            }
            catch(ThreadAbortException)
            {
                // It's likely that this was raised from the thread aborting, so just do nothing.
            }
        }

        /// <summary>
        /// Disconnects the TCP client, cleans up any threads and raises the appropriate events.
        /// </summary>
        public void Disconnect()
        {
            if (dataThread != null)
            {
                if (dataThread.ThreadState != System.Threading.ThreadState.Aborted)
                {
                    dataThread.Abort();
                }
                if (dataThread != null)
                {
                    dataThread = null;
                }
            }
            Client.Client.Disconnect(false);

            if (Disconnected != null)
            {
                Disconnected();
            }
        }

    }
}
