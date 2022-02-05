using System;
using MoonlapseNetworking.Packets;

namespace MoonlapseNetworking
{
    /// <summary>
    /// By default, an incoming packet is simply absorbed and not handled.
    /// It is up to an inheriting state class to subscribe to the event
    /// handling functionality.
    ///
    /// <br></br>
    ///
    /// To register a handler for packet p, you have to add case to HandlePacketFromString
    /// as well as add a PacketEventHandler and register it in child class
    /// </summary>
    public abstract class State
    {
        protected delegate void PacketEventHandler(object sender, PacketEventArgs args);

        public State()
        {
        }

        protected event PacketEventHandler LoginPacketEvent, ChatPacketEvent, RegisterPacketEvent;

        public void HandlePacketFromString(string packetString)
        {
            var typeString = packetString[0..packetString.IndexOf(':')];
            typeString = $"MoonlapseServer.Models.Packets.{typeString}";
            var type = Type.GetType(typeString);

            // switch statement won't work here
            if (type == typeof(LoginPacket))
            {
                LoginPacketEvent.Invoke(this, new PacketEventArgs(packetString));
            }
            else if (type == typeof(ChatPacket))
            {
                ChatPacketEvent.Invoke(this, new PacketEventArgs(packetString));
            }
            else if (type == typeof(RegisterPacket))
            {
                RegisterPacketEvent.Invoke(this, new PacketEventArgs(packetString));
            }
            else
            {
                throw new Exception($"Packet type ({typeString}) doesn't exist");
            }
        }
    }

    public class PacketEventArgs : EventArgs
    {
        public string PacketString { get; }

        public PacketEventArgs(string s)
        {
            PacketString = s;
        }
    }
}
