using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("Log")]
    public class Log
    {
        [Key]
        [Required]
        public long IdLog { get; set; }
        [Required]
        public string Error { get; set; }
        [Required]
        public string Message { get; set; }

    }
}
