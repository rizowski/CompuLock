using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Imports.Newtonsoft.Json;

namespace REST
{
    public class Error
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
