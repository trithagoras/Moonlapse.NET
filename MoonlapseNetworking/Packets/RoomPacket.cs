using System;
using MoonlapseNetworking.Models;

namespace MoonlapseNetworking.Packets
{
    public class RoomPacket : Packet
    {
        public Room Room { get; set; }
    }
}
