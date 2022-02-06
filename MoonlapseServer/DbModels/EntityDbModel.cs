using System;
using System.ComponentModel.DataAnnotations;

namespace MoonlapseServer.DbModels
{
    public class EntityDbModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string TypeName { get; set; }
    }
}
