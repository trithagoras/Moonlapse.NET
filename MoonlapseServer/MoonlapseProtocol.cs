using System;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using MoonlapseServer.States;
using MoonlapseNetworking;
using MoonlapseServer.Utils.Logging;
using MoonlapseNetworking.Packets;
using MoonlapseNetworking.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoonlapseNetworking.Models.Components;

namespace MoonlapseServer
{
    public class Protocol
    {
        readonly TcpClient _client;
        public Server Server { get; }

        public State State { get; set; }

        public Entity PlayerEntity { get; set; }

        public Protocol(TcpClient client, Server server)
        {
            _client = client;
            Server = server;
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
            Server.LoadEntity(PlayerEntity);

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

        public void SendPacket(Packet p)
        {
            try
            {
                var sw = new StreamWriter(_client.GetStream());
                sw.WriteLine(p.ToString());
                sw.Flush();

            }
            catch (Exception)
            {
                Log($"Problem sending {p} to client", LogContext.Error);
            }
        }

        public void Broadcast(Packet p)
        {
            // todo: add include list param. For now, this excludes self
            foreach (var proto in Server.ConnectedProtocols)
            {
                if (proto == this)
                {
                    continue;
                }

                proto.SendPacket(p);
            }
        }

        void ConnectionEnded()
        {
            Log("Client disconnected");
            Server.ConnectedProtocols.Remove(this);
            Broadcast(new PlayerLeftPacket { EntityId = PlayerEntity.Id } );
        }

        public void Log(string message, LogContext context = LogContext.Info)
        {
            Logging.Log(PlayerEntity == null ? "None" : PlayerEntity.Name, message, context, State.GetType().ToString());
        }
    }
}
