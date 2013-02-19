using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Database.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public List<Computer> Computers { get; set; }
        public string AuthToken { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User()
        {
            
        }
    }
}
