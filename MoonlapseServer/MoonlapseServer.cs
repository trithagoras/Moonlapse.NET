using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using MoonlapseServer.Utils.Logging;
using MoonlapseNetworking.ServerModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MoonlapseServer
{
    public class Server
    {
        public const string Host = "127.0.0.1";
        public const int Port = 42523;

        public readonly ISet<Protocol> ConnectedProtocols;

        readonly TcpListener _listener;

        public Dictionary<int, Entity> RoomEntities { get; }

        public Server()
        {
            var ip = IPAddress.Parse(Host);
            ConnectedProtocols = new HashSet<Protocol>();
            _listener = new TcpListener(ip, Port);
            RoomEntities = new Dictionary<int, Entity>();
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

        /// <summary>
        /// Loads ALL components from DB that belongs to entity and sets them.
        /// </summary>
        /// <param name="entity"></param>
        public static void LoadEntity(Entity entity)
        {
            var db = new MoonlapseDbContext();

            var components = db.Components
                .Include(c => c.Entity)
                .Where(c => c.Entity == entity)
                .ToList();

            foreach (var component in components)
            {
                entity.SetComponent(component);
            }
        }

        static void Log(string message, LogContext context = LogContext.Info)
        {
            Logging.Log("SERVER", message, context, "NA");
        }
    }
}