using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using MoonlapseServer.Utils.Logging;
using MoonlapseNetworking.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MoonlapseNetworking.Models.Components;

namespace MoonlapseServer
{
    public class Server
    {
        public const string Host = "127.0.0.1";
        public const int Port = 42523;

        public readonly ISet<Protocol> ConnectedProtocols;

        public Dictionary<int, Room> Rooms;

        public readonly MoonlapseDbContext Db;
        const int DbSaveChangesTimer = 30;  // Db changes are saved every 30s

        readonly TcpListener _listener;

        public Server()
        {
            var ip = IPAddress.Parse(Host);
            ConnectedProtocols = new HashSet<Protocol>();

            Db = new MoonlapseDbContext();
            SaveChangesToDb();

            Rooms = new Dictionary<int, Room>();
            LoadRooms();
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

        public void LoadRooms()
        {
            var rooms = Db.Rooms
                .ToList();

            foreach (var room in rooms)
            {
                Rooms[room.Id] = room;
                room.Unpack();

            }
        }

        /// <summary>
        /// Loads ALL components from DB that belongs to entity and sets them.
        /// </summary>
        /// <param name="entity"></param>
        public void LoadEntity(Entity entity)
        {
            var components = Db.Components
                .Include(c => c.Entity)
                .Include(pos => (pos as Position).Room)
                .Where(c => c.Entity == entity)
                .ToList();

            foreach (var component in components)
            {
                entity.SetComponent(component);
            }
        }

        async Task SaveChangesToDb()
        {
            while (true)
            {
                await Task.Delay(DbSaveChangesTimer * 1000);
                try
                {
                    var changes = await Db.SaveChangesAsync();
                    Log($"Saved {changes} changes to db");

                }
                catch (Exception e)
                {
                    Log(e.Message);
                }
            }
            
        }

        static void Log(string message, LogContext context = LogContext.Info)
        {
            Logging.Log("SERVER", message, context, "NA");
        }
    }
}