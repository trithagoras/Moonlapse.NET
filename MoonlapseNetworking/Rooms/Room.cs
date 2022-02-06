using System;
using System.Collections.Generic;
using MoonlapseNetworking.ServerModels;

namespace MoonlapseNetworking.Rooms
{
    public class Room
    {
        public Dictionary<int, Entity> Entities;

        public Room()
        {
            Entities = new Dictionary<int, Entity>();
        }
    }
}
