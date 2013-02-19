﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace REST.Models
{
    [JsonObject]
    public class Computer
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "enviroment")]
        public string Enviroment { get; set; }
        [JsonProperty(PropertyName = "ip_address")]
        public string IpAddress { get; set; }

        [JsonProperty(PropertyName = "account_attributes", NullValueHandling = NullValueHandling.Ignore)]
        public virtual List<REST.Models.Account> Accounts { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }

        public Computer(int id, int userId, string name, string enviroment, string ipAddress, List<REST.Models.Account> accounts )
        {
            Id = id;
            UserId = userId;
            Name = name;
            Enviroment = enviroment;
            IpAddress = ipAddress;
            Accounts = accounts;
        }
        public Computer()
        {

        }

        public Computer(int id, int userId, string name, string enviroment, string ipAddress)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Enviroment = enviroment;
            IpAddress = ipAddress;
        }

        public Computer( int userId, string name, string enviroment, string ipAddress)
        {
            UserId = userId;
            Name = name;
            Enviroment = enviroment;
            IpAddress = ipAddress;
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
