using System;
using MoonlapseNetworking;
using MoonlapseNetworking.Packets;
using MoonlapseNetworking.ServerModels;
using MoonlapseClient.Consoles;
using SadConsole;
using System.Collections.Generic;

namespace MoonlapseClient.States
{
    public class GameState : State
    {
        readonly GameConsole _console;
        readonly NetworkController _networkController;

        public Dictionary<int, Entity> KnownEntities { get; }

        public Entity PlayerEntity { get; private set; }

        public GameState(NetworkController nc)
        {
            _networkController = nc;
            _console = new GameConsole(this)
            {
                IsFocused = true
            };

            KnownEntities = new Dictionary<int, Entity>();

            EntityPacketEvent += GameState_EntityPacketEvent;
            ComponentPacketEvent += GameState_ComponentPacketEvent;
            _ = _networkController.SendPacket(new OkPacket());

            Global.CurrentScreen = _console;
        }

        private void GameState_ComponentPacketEvent(object sender, PacketEventArgs args)
        {
            var p = Packet.FromString<ComponentPacket>(args.PacketString);
            var c = p.Component;
            var e = KnownEntities[c.Entity.Id];
            e.SetComponent(c);
        }

        private void GameState_EntityPacketEvent(object sender, PacketEventArgs args)
        {
            var p = Packet.FromString<EntityPacket>(args.PacketString);
            var e = p.Entity;
            KnownEntities[e.Id] = e;

            // The player is the first entity that is sent by server.
            PlayerEntity ??= e;
        }
    }
}
