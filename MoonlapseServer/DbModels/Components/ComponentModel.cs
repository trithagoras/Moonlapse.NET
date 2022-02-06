using System;
using System.ComponentModel.DataAnnotations;

namespace MoonlapseServer.DbModels.Components
{
    public class ComponentModel
    {
        public int Id { get; set; }

        [Required]
        public EntityDbModel Entity { get; set; }

        public string TypeName { get; set; }
    }
}
