using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("Fact_Encabezado")]
    public partial class FactEncabezado
    {
        [Key]
        [Required]
        public int FEnc_ID { get; set; }

        [Required]
        public int FEnc_Numero { get; set; }

        [Required]
        public DateTime FEnc_Fecha { get; set; }

        [Required]
        public int zPer_ID { get; set; }
    }
}
