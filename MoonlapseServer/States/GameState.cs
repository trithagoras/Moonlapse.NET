using System;
using MoonlapseNetworking;
using MoonlapseNetworking.Packets;
using MoonlapseNetworking.ServerModels;
using MoonlapseNetworking.ServerModels.Components;

namespace MoonlapseServer.States
{
    public class GameState : State
    {
        Protocol _protocol;

        public GameState(Protocol protocol)
        {
            _protocol = protocol;
            ChatPacketEvent += MainState_ChatPacketEvent;

            _protocol.SendPacket(new EntityPacket { Entity = _protocol.PlayerEntity });
            _protocol.SendPacket(new ComponentPacket { Component = _protocol.PlayerEntity.GetComponent<Position>() });
        }

        void MainState_ChatPacketEvent(object sender, PacketEventArgs args)
        {
            var p = Packet.FromString<ChatPacket>(args.PacketString);

            _protocol.Log($"Sent chat: {p.Message}");
        }
    }
}
