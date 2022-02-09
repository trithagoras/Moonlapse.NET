using System;
using MoonlapseNetworking;
using MoonlapseNetworking.Packets;
using MoonlapseNetworking.Models;
using MoonlapseClient.Consoles;
using SadConsole;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MoonlapseNetworking.Models.Components;
using Microsoft.Xna.Framework;
using SadConsole.Input;

namespace MoonlapseClient.States
{
    public class GameState : State
    {
        readonly GameConsole _console;
        readonly NetworkController _networkController;

        public Dictionary<int, Entity> KnownEntities { get; }

        public Room Room;

        public Entity PlayerEntity { get; private set; }

        public GameState(NetworkController nc)
        {
            _networkController = nc;
            _console = new GameConsole(this)
            {
                IsFocused = true
            };

            KnownEntities = new Dictionary<int, Entity>();

            // todo: TESTING ONLY: THIS SHOULD BE SENT AS A PACKET
            Room = new Room
            {
                Name = "Garden"
            };
            Room.Unpack();

            EntityPacketEvent += GameState_EntityPacketEvent;
            ComponentPacketEvent += GameState_ComponentPacketEvent;
            _ = _networkController.SendPacket(new OkPacket());

            Global.CurrentScreen = _console;
            SadConsole.Game.OnUpdate = OnUpdate;
        }

        void GameState_ComponentPacketEvent(object sender, PacketEventArgs args)
        {
            var p = Packet.FromString<ComponentPacket>(args.PacketString);
            var c = p.Component;
            var e = KnownEntities[c.Entity.Id];
            e.SetComponent(c);
        }

        void GameState_EntityPacketEvent(object sender, PacketEventArgs args)
        {
            var p = Packet.FromString<EntityPacket>(args.PacketString);
            var e = p.Entity;
            KnownEntities[e.Id] = e;

            // The player is the first entity that is sent by server.
            PlayerEntity ??= e;
        }

        void OnUpdate(GameTime gameTime)
        {
            GetInput();
        }

        void GetInput()
        {
            // todo: need this sorted
            if (PlayerEntity == null || PlayerEntity.GetComponent<Position>() == null)
            {
                return;
            }
            var keyboardState = SadConsole.Global.KeyboardState;
            var dx = (keyboardState.IsKeyPressed(Keys.Right) ? 1 : 0) - (keyboardState.IsKeyPressed(Keys.Left) ? 1 : 0);
            var dy = (keyboardState.IsKeyPressed(Keys.Down) ? 1 : 0) - (keyboardState.IsKeyPressed(Keys.Up) ? 1 : 0);

            // send packet to server
            if (dx != 0 || dy != 0)
            {
                var movePacket = new MovePacket
                {
                    Dx = dx,
                    Dy = dy
                };
                _ = _networkController.SendPacket(movePacket);

                var pos = PlayerEntity.GetComponent<Position>();
                pos.X += dx;
                pos.Y += dy;
            }
            
        }
    }
}
