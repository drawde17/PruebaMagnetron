using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("Fact_Detalle")]
    public partial class FactDetalle
    {
        [Key]
        [Required]
        public int FDet_ID { get; set; }

        [Required]
        public string FDet_Linea { get; set; }

        [Required]
        public int FDet_Cantidad { get; set; }

        [Required]
        public int zProd_ID { get; set; }

        [Required]
        public int zFEnc_ID { get; set; }
    }
}
