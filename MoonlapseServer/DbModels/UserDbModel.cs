using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoonlapseServer.DbModels
{
    public class UserDbModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("EntityId")]
        public EntityDbModel Entity { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}