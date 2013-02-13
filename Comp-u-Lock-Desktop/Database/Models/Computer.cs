using System;
using System.Collections.Generic;

namespace Database.Models
{
    public class Computer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Enviroment { get; set; }
        public string IpAddress { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

    }
}
