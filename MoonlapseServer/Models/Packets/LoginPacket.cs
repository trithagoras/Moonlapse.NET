using System;
namespace MoonlapseServer.Models.Packets
{
    public class LoginPacket : Packet
    {
        public string Username, Password;
    }
}
