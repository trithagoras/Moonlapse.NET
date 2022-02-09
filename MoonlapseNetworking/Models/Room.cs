using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Newtonsoft.Json;

namespace MoonlapseNetworking.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } // to find data in Rooms/[Name]/

        [NotMapped]
        [JsonIgnore]
        public Dictionary<int, Entity> Entities;

        [NotMapped]
        [JsonIgnore]
        public bool Unpacked { get; private set; }

        [NotMapped]
        [JsonIgnore]
        public int Width { get; private set; }

        [NotMapped]
        [JsonIgnore]
        public int Height { get; private set; }

        [NotMapped]
        [JsonIgnore]
        public int[,] Terrain { get; private set; }

        public Room()
        {
            Entities = new Dictionary<int, Entity>();
        }

        public void Unpack()
        {
            // To know: [project]/Assets/Rooms/[Name] exists physically in MoonlapseNetworking, but a link
            // is added to MoonlapseClient and MoonlapseServer. Any changes to the physical file will
            // cascade to the other projects.

            var buildDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = System.IO.Path.Join(buildDir, "Assets", "Rooms", Name, "Terrain.csv");

            var lines = System.IO.File.ReadAllLines(path);
            Width = lines[0].Split(',').Length;
            Height = lines.Length;
            Terrain = new int[Height, Width];

            for (var row = 0; row < Height; row++)
            {
                var line = lines[row];
                var values = line.Split(',');

                for (var col = 0; col < Width; col++)
                {
                    Terrain[row, col] = int.Parse(values[col]) - 1;
                }
            }


            Unpacked = true;
        }

        public bool CoordinateExists(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
}
