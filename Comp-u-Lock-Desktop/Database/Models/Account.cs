using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }

        public string Domain { get; set; }
        public bool Tracking { get; set; }

        public TimeSpan AllottedTime { get; set; }
        public TimeSpan UsedTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateUpdated { get; set; }

        public virtual ICollection<History> Histories { get; set; }
        public virtual ICollection<Process> Processes { get; set; }
        public virtual ICollection<Program> Programs { get; set; }
    }
}
