using System;
namespace MoonlapseNetworking.Packets
{
    public class PlayerLeftPacket : Packet
    {
        public int EntityId { get; set; }
    }
}
