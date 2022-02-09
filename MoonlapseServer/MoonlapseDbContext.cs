using System;
using Microsoft.EntityFrameworkCore;
using MoonlapseNetworking.Models;
using MoonlapseNetworking.Models.Components;

namespace MoonlapseServer
{
    public class MoonlapseDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Entity> Entities { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Position> PositionComponents { get; set; }

        public string DbPath { get; }

        public MoonlapseDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "moonlapse.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
