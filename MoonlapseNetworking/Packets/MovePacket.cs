using System;
namespace MoonlapseNetworking.Packets
{
    public class MovePacket : Packet
    {
        public int Dx { get; set; }

        public int Dy { get; set; }
    }
}
