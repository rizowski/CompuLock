using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Database.Models
{
    public class Account
    {
        public Account()
        {
        }

        public int Id { get; set; }

        public int ComputerId { get; set; }

        public string Domain { get; set; }
        public string Username { get; set; }

        public bool Tracking { get; set; }
        public int AllottedTime { get; set; }
        public int UsedTime { get; set; }

        public List<History> Histories { get; set; }
        public List<Process> Processes { get; set; }
        public List<Program> Programs { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
