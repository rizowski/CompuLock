using System;

namespace Database.Models
{
    public class Program
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OpenCount { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
