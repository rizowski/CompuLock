using System;
using System.Collections.Generic;

namespace Database.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string AuthToken { get; set; }

        public DateTime LastUpdated { get; set; }
        public DateTime DateCreated { get; set; }
        
    }
}
