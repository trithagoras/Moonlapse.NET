using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoonlapseServer.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        public EntityModel Entity { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}