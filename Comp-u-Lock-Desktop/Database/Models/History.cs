using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class History
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Domain { get; set; }
        public string Url { get; set; }
        public int VisitCount { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        
    }
}
