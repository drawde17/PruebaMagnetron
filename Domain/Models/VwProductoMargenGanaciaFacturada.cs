using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public partial class VwProductoMargenGanaciaFacturada
    {
        public int Prod_ID { get; set; }
        public string Prod_Descripcion { get; set; }
        public decimal Prod_Precio { get; set; }
        public decimal Prod_Costo { get; set; }
        public string Prod_UM { get; set; }
        public decimal MargenDeGanancia { get; set; }
    }
}
