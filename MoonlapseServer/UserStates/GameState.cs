using System;
using MoonlapseNetworking;
using MoonlapseNetworking.Packets;
using MoonlapseNetworking.Models.Components;

namespace MoonlapseServer.UserStates
{
    public class GameState : UserState
    {
        Protocol _protocol;

        public GameState(Protocol protocol)
        {
            _protocol = protocol;
            ChatPacketEvent += MainState_ChatPacketEvent;
            MovePacketEvent += GameState_MovePacketEvent;

            RoomEntered();
        }

        void GameState_MovePacketEvent(object sender, PacketEventArgs args)
        {
            // todo: check for collision :woozy_face:
            var p = Packet.FromString<MovePacket>(args.PacketString);
            _protocol.Log($"Received Move Packet: ({p.Dx},{p.Dy})");

            var pos = _protocol.PlayerEntity.GetComponent<Position>();
            pos.X += p.Dx;
            pos.Y += p.Dy;

            // broadcast new position to other entities (todo: only within our viewport)
            _protocol.Broadcast(new ComponentPacket { Component = pos });
        }

        void MainState_ChatPacketEvent(object sender, PacketEventArgs args)
        {
            var p = Packet.FromString<ChatPacket>(args.PacketString);

            _protocol.Log($"Sent chat: {p.Message}");
        }

        // This gets called whenever entering room for first time.
        void RoomEntered()
        {
            // todo: this better

            var ep = new EntityPacket { Entity = _protocol.PlayerEntity };
            var cp = new ComponentPacket { Component = _protocol.PlayerEntity.GetComponent<Position>() };

            _protocol.SendPacket(ep);
            _protocol.SendPacket(cp);
            _protocol.Broadcast(ep);
            _protocol.Broadcast(cp);

            var room = _protocol.Server.Rooms[(cp.Component as Position).Room.Id];
            room.Entities[ep.Entity.Id] = ep.Entity;

            foreach (var entity in room.Entities.Values)
            {
                if (entity == _protocol.PlayerEntity)
                {
                    continue;
                }

                _protocol.SendPacket(new EntityPacket { Entity = entity });

                // todo: consider foreach c in components: sendpacket ??
                _protocol.SendPacket(new ComponentPacket { Component = entity.GetComponent<Position>() });
            }
        }
    }
}
