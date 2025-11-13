using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class FacturaDTO
    {
        public int FEnc_ID { get; set; }

        [Required]
        public int FEnc_Numero { get; set; }

        [Required]
        public DateTime FEnc_Fecha { get; set; }

        [Required]
        public int zPer_ID { get; set; }

        [Required]
        public IEnumerable<FacturaDetalleDTO> FacturaDetalle { get; set; }
    }

    public class FacturaDetalleDTO
    {
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
