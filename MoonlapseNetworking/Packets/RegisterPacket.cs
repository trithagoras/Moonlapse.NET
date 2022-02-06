using System;
using Newtonsoft.Json;

namespace MoonlapseNetworking.Packets
{
    public class RegisterPacket : Packet
    {
        [JsonProperty(Required = Required.Always)]
        public string Username { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Password { get; set; }
    }
}
