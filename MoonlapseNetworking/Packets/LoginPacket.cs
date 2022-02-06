using System;
using Newtonsoft.Json;
namespace MoonlapseNetworking.Packets
{
    public class LoginPacket : Packet
    {
        [JsonProperty(Required = Required.Always)]
        public string Username { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Password { get; set; }
    }
}
