using System;
using System.Collections.Generic;

namespace MoonlapseServer.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public EntityModel Entity { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}