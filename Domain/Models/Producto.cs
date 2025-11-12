using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("Producto")]
    public partial class Producto
    {
        [Key]
        [Required]
        public int Prod_ID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Prod_Descripcion { get; set; }

        [Required]
        public decimal Prod_Precio { get; set; }

        [Required]
        public decimal Prod_Costo { get; set; }

        [Required]
        public string Prod_UM { get; set; }

    }
}
