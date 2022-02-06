using System;
using Newtonsoft.Json;
using System.Linq;

namespace MoonlapseNetworking.Packets
{
    public abstract class Packet
    {
        public override string ToString()
        {
            string obj = JsonConvert.SerializeObject(this);
            return $"{GetType().Name}:{obj}";
        }

        public static T FromString<T>(string s) where T : Packet
        {
            var objString = s[(s.IndexOf(':') + 1)..];
            var obj = JsonConvert.DeserializeObject<T>(objString, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

            if (obj == null)
            {
                throw new JsonException("Object null");
            }

            return obj;
        }
    }
}
