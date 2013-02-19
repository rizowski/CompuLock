using System;
using Newtonsoft.Json;

namespace Database.Models
{
    public class Program
    {
        public int Id { get; set; }

        public int AccountId { get; set; }
        public string Name { get; set; }
        public int OpenCount { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
