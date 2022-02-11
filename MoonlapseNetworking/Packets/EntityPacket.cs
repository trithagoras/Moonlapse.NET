using System;
using MoonlapseNetworking.Models;
using Newtonsoft.Json;

namespace MoonlapseNetworking.Packets
{
    public class EntityPacket : Packet
    {
        public Entity Entity { get; set; }
    }
}
