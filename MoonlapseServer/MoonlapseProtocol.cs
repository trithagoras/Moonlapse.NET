using System;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using MoonlapseServer.States;
using MoonlapseNetworking;
using MoonlapseServer.Utils.Logging;
using MoonlapseNetworking.Packets;
using MoonlapseNetworking.ServerModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoonlapseNetworking.ServerModels.Components;

namespace MoonlapseServer
{
    public class Protocol
    {
        readonly TcpClient _client;
        readonly Server _server;

        public State State { get; set; }

        public Entity PlayerEntity { get; set; }

        public Protocol(TcpClient client, Server server)
        {
            _client = client;
            _server = server;
            State = new EntryState(this);
        }

        public async Task Start()
        {
            var stream = _client.GetStream();
            var sr = new StreamReader(stream);

            // main read loop
            string line;
            try
            {
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    StringRecieved(line);
                }
            } catch (Exception)
            {
            }

            // connection ended
            ConnectionEnded();
        }

        public void Login(Entity playerEntity)
        {
            PlayerEntity = playerEntity;

            // need to add all components to player entity (and send later)
            LoadEntity(PlayerEntity);

            Log("Successfully logged in");
            SendPacket(new OkPacket { Message = "Login" });
        }

        void StringRecieved(string s)
        {
            try
            {
                State.HandlePacketFromString(s);
            }
            catch (PacketEventNotSubscribedException e)
            {
                Log(e.Message, LogContext.Warn);
            }
            catch (Exception e)
            {
                Log($"Malformed message received: {s} - inner details: {e.Message}", LogContext.Error);
            }
        }

        public async void SendPacket(Packet p)
        {
            try
            {
                var sw = new StreamWriter(_client.GetStream());
                await sw.WriteLineAsync(p.ToString());
                await sw.FlushAsync();

            }
            catch (Exception)
            {
                Log($"Problem sending {p} to client", LogContext.Error);
            }
        }

        void ConnectionEnded()
        {
            Log("Client disconnected");
            _server.ConnectedProtocols.Remove(this);
        }

        public void Log(string message, LogContext context = LogContext.Info)
        {
            Logging.Log(PlayerEntity == null ? "None" : PlayerEntity.Name, message, context, State.GetType().ToString());
        }

        /// <summary>
        /// Loads ALL components from DB that belongs to entity and sets them.
        /// </summary>
        /// <param name="entity"></param>
        void LoadEntity(Entity entity)
        {
            var db = new MoonlapseDbContext();

            var componentModels = db.Components
                .Include(c => c.Entity)
                .Where(cm => cm.Entity.Id == entity.Id)
                .ToList();

            // TODO: This will become VERY unmanageable as # of components grow
            foreach (var model in componentModels)
            {
                if (model.TypeName == "Position")
                {
                    var posModel = db.PositionComponents
                        .Include(p => p.Component)
                        .Where(p => p.Component == model)
                        .First();

                    var position = new Position
                    {
                        EntityId = entity.Id,
                        X = posModel.X,
                        Y = posModel.Y
                    };

                    entity.SetComponent(position);
                }
            }
        }
    }
}
