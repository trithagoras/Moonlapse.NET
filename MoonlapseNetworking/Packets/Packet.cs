using System;
using Newtonsoft.Json;

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
            return JsonConvert.DeserializeObject<T>(objString);
        }
    }
}
