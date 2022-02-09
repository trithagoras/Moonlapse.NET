using System;
using MoonlapseNetworking;
using MoonlapseNetworking.Packets;
using MoonlapseNetworking.Models;
using MoonlapseNetworking.Models.Components;

namespace MoonlapseServer.States
{
    public class GameState : State
    {
        Protocol _protocol;

        // Room => _protocol.PlayerEntity.GetComponent<Position>().Room;

        public GameState(Protocol protocol)
        {
            _protocol = protocol;
            ChatPacketEvent += MainState_ChatPacketEvent;
            MovePacketEvent += GameState_MovePacketEvent;

            _protocol.SendPacket(new EntityPacket { Entity = _protocol.PlayerEntity });
            _protocol.SendPacket(new ComponentPacket { Component = _protocol.PlayerEntity.GetComponent<Position>() });

            RoomEntered();
        }

        private void GameState_MovePacketEvent(object sender, PacketEventArgs args)
        {
            // todo: check for collision :woozy_face:
            var p = Packet.FromString<MovePacket>(args.PacketString);
            _protocol.Log($"Received Move Packet: ({p.Dx},{p.Dy})");

            var pos = _protocol.PlayerEntity.GetComponent<Position>();
            pos.X += p.Dx;
            pos.Y += p.Dy;
        }

        void MainState_ChatPacketEvent(object sender, PacketEventArgs args)
        {
            var p = Packet.FromString<ChatPacket>(args.PacketString);

            _protocol.Log($"Sent chat: {p.Message}");
        }

        // This gets called whenever entering room for first time.
        void RoomEntered()
        {
            foreach (var entity in _protocol.Server.RoomEntities.Values)
            {
                if (entity == _protocol.PlayerEntity)
                {
                    continue;
                }

                _protocol.SendPacket(new EntityPacket { Entity = entity });

                // foreach c in components: sendpacket ??
                _protocol.SendPacket(new ComponentPacket { Component = entity.GetComponent<Position>() });
            }
        }
    }
}
