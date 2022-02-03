using System;
using MoonlapseServer.Models.Packets;

namespace MoonlapseServer.States
{
    public class MainState : State
    {
        public MainState(Protocol protocol) : base(protocol)
        {
        }

        protected override void HandleChatPacket(ChatPacket p)
        {
            _protocol.Log($"Chat sent: {p.Message}");
        }
    }
}
