using System;
namespace MoonlapseServer.Models.Packets
{
    public class RegisterPacket : Packet
    {
        public string Username, Password;
    }
}
