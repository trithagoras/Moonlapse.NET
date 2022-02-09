using System;
using Newtonsoft.Json;
using MoonlapseNetworking.Models.Components;
namespace MoonlapseNetworking.Packets
{
    public class ComponentPacket : Packet
    {
        public Component Component { get; set; }

        public override string ToString()
        {
            string obj = JsonConvert.SerializeObject(this, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            return $"{GetType().Name}:{obj}";
        }
    }
}
