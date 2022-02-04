using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using MoonlapseServer.Utils.Logging;

namespace MoonlapseServer
{
    public class Server
    {
        public const string Host = "127.0.0.1";
        public const int Port = 42523;

        public readonly ISet<Protocol> ConnectedProtocols;

        readonly TcpListener _listener;

        public Server()
        {
            var ip = IPAddress.Parse(Host);
            ConnectedProtocols = new HashSet<Protocol>();
            _listener = new TcpListener(ip, Port);
        }

        public async Task Start()
        {
            Log("Starting MoonlapseServer");
            _listener.Start();

            // main accept loop
            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync();
                Log("Client connected");
                var proto = new Protocol(client, this);
                ConnectedProtocols.Add(proto);
                _ = Task.Run(proto.Start);
            }
        }

        static void Log(string message, LogContext context = LogContext.Info)
        {
            Logging.Log("SERVER", message, context);
        }
    }
}