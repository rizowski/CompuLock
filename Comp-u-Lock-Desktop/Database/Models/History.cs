﻿using System;
using Newtonsoft.Json;

namespace Database.Models
{
    public class History
    {
        public int Id { get; set; }

        public int AccountId { get; set; }
        public string Domain { get; set; }
        public string Url { get; set; }
        public int VisitCount { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
