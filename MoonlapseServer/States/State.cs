using System;
using MoonlapseServer;
using MoonlapseServer.Models.Packets;
using MoonlapseServer.Utils.Logging;

namespace MoonlapseServer.States
{
    /// <summary>
    /// By default, an incoming packet is simply absorbed and not handled.
    /// It is up to an inheriting state class to override the packet
    /// handling functionality.
    /// </summary>
    public abstract class State
    {
        protected readonly Protocol _protocol;

        public State(Protocol protocol)
        {
            _protocol = protocol;
        }

        public void HandlePacketFromString(string packetString)
        {
            var typeString = packetString[0..packetString.IndexOf(':')];
            typeString = $"MoonlapseServer.Models.Packets.{typeString}";
            var type = Type.GetType(typeString);

            // switch statement won't work here
            if (type == typeof(LoginPacket))
            {
                HandleLoginPacket(Packet.FromString<LoginPacket>(packetString));
            }
            else if (type == typeof(ChatPacket))
            {
                HandleChatPacket(Packet.FromString<ChatPacket>(packetString));
            }
            else
            {
                throw new Exception($"Packet type ({typeString}) doesn't exist");
            }
        }

        protected virtual void HandleLoginPacket(LoginPacket p)
        {
            _protocol.Log($"{p.GetType()} handler not registered for {GetType()}", LogContext.Error);
        }

        protected virtual void HandleChatPacket(ChatPacket p)
        {
            _protocol.Log($"{p.GetType()} handler not registered for {GetType()}", LogContext.Error);
        }
    }
}
