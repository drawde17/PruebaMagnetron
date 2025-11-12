using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public partial class PersonaDTO
    {
        public int Per_ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Per_Nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string Per_Apellido { get; set; }

        [Required]
        public int Per_TipoDocumento { get; set; }

        [Required]
        [MaxLength(15)]
        public string Per_Documento { get; set; }
    }
}
