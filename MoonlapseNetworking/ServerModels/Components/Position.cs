using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoonlapseNetworking.ServerModels.Components
{
    public class Position : Component
    {
        public int X { get; set; }

        public int Y { get; set; }
    }
}
