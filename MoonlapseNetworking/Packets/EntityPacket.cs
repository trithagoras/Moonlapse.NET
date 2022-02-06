using System;
using MoonlapseNetworking.ServerModels;
using Newtonsoft.Json;

namespace MoonlapseNetworking.Packets
{
    public class EntityPacket : Packet
    {
        public Entity Entity { get; set; }

        public override string ToString()
        {
            string obj = JsonConvert.SerializeObject(this, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Arrays });
            return $"{GetType().Name}:{obj}";
        }
    }
}
