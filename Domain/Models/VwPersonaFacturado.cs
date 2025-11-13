using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public partial class VwPersonaFacturado
    {
        public string Per_Nombre { get; set; }
        public string Per_Apellido { get; set; }
        public string TDoc_Nombre { get; set; }
        public string Per_Documento { get; set; }
        public decimal Facturado { get; set; }
    }
}
