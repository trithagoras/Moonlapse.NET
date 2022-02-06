using System;
using System.ComponentModel.DataAnnotations;

namespace MoonlapseServer.DbModels.Components
{
    public class PositionComponent
    {
        public int Id { get; set; }

        [Required]
        public ComponentModel Component { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
