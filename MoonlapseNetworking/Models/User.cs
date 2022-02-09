using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoonlapseNetworking.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("EntityId")]
        public Entity Entity { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
