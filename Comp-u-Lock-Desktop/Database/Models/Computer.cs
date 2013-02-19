using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Database.Models
{
    public class Computer
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Enviroment { get; set; }
        public string IpAddress { get; set; }

        public virtual List<Account> Accounts { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Computer()
        {

        }
    }
}
