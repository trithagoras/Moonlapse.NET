using System;
using MoonlapseNetworking;
using MoonlapseNetworking.Packets;

namespace MoonlapseServer.States
{
    public class MainState : State
    {
        Protocol _protocol;

        public MainState(Protocol protocol)
        {
            _protocol = protocol;
            ChatPacketEvent += MainState_ChatPacketEvent;
        }

        void MainState_ChatPacketEvent(object sender, PacketEventArgs args)
        {
            var p = Packet.FromString<ChatPacket>(args.PacketString);

            _protocol.Log($"Sent chat: {p.Message}");
        }
    }
}
