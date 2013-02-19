using System;
using Newtonsoft.Json;

namespace Database.Models
{
    public class Process
    {
        public int Id { get; set; }

        public int AccountId { get; set; }
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
