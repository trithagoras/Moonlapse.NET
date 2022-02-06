using System;
namespace MoonlapseNetworking.Packets
{
    public class DenyPacket : Packet
    {
        public string Message { get; set; }
    }
}
