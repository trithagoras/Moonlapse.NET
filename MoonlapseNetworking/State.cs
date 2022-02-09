using System;
using MoonlapseNetworking.Packets;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MoonlapseNetworking
{
    /// <summary>
    /// By default, an incoming packet is simply absorbed and not handled.
    /// It is up to an inheriting state class to subscribe to the event
    /// handling functionality.
    ///
    /// <br></br>
    ///
    /// To register a handler for packet p, you have to add to map local var
    /// as well as add a PacketEventHandler and register it in child class
    /// </summary>
    public abstract class State
    {
        protected delegate void PacketEventHandler(object sender, PacketEventArgs args);

        protected event PacketEventHandler LoginPacketEvent, ChatPacketEvent, RegisterPacketEvent,
            DenyPacketEvent, OkPacketEvent, EntityPacketEvent, ComponentPacketEvent, MovePacketEvent;

        public void HandlePacketFromString(string packetString)
        {
            var typeString = packetString[0..packetString.IndexOf(':')];
            typeString = $"MoonlapseNetworking.Packets.{typeString}";
            var type = Type.GetType(typeString);

            Dictionary<Type, PacketEventHandler> map = new()
            {
                { typeof(LoginPacket), LoginPacketEvent },
                { typeof(RegisterPacket), RegisterPacketEvent },
                { typeof(ChatPacket), ChatPacketEvent },
                { typeof(DenyPacket), DenyPacketEvent },
                { typeof(OkPacket), OkPacketEvent },
                { typeof(EntityPacket), EntityPacketEvent },
                { typeof(ComponentPacket), ComponentPacketEvent },
                { typeof(MovePacket), MovePacketEvent }
            };

            try
            {
                var handler = map[type];
                handler.Invoke(this, new PacketEventArgs(packetString));
            }
            catch (KeyNotFoundException)
            {
                throw new Exception($"Packet type ({typeString}) doesn't exist");
            }
            catch (ArgumentNullException)
            {
                throw new Exception($"Packet type ({typeString}) doesn't exist");
            }
            catch (NullReferenceException)
            {
                throw new PacketEventNotSubscribedException(typeString);
            }
            catch (JsonException)
            {
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

    public class PacketEventNotSubscribedException : Exception
    {
        public PacketEventNotSubscribedException(string packetType) : base($"{packetType} not registered in this state")
        {
        }
    }
}
