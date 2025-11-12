using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public partial class MagnetronContext : DbContext
    {
        public MagnetronContext() {}

        public MagnetronContext(DbContextOptions<MagnetronContext> options) : base(options) { }

        public DbSet<Persona> Persona { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<FactEncabezado> FactEncabezado { get; set; }
        public DbSet<FactDetalle> FactDetalle { get; set; }
        public DbSet<Log> Log { get; set; }



    }
}
