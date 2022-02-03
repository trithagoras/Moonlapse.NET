using System;
using MoonlapseServer.Models.Packets;

namespace MoonlapseServer.States
{
    public class EntryState : State
    {
        public EntryState(Protocol protocol) : base(protocol)
        {
        }

        protected override void HandleLoginPacket(LoginPacket p)
        {
            _protocol.Login(p.Username);
            _protocol.State = new MainState(_protocol);
        }
    }
}
