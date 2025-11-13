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
        public DbSet<VwPersonaFacturado> VwPersonaFacturado { get; set; }
        public DbSet<VwPersonaProductoCaro> View_PersonaProductoCaro { get; set; }
        public DbSet<VwProductoCantidadFacturada> VwProductoCantidadFacturada { get; set; }
        public DbSet<VwProductoMargenGanaciaFacturada> VwProductoMargenGanaciaFacturada { get; set; }
        public DbSet<VwProductoUtilidadFacturada> VwProductoUtilidadFacturada { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<VwPersonaFacturado>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("View_PersonaFacturado");
                entity.Property(e => e.Per_Nombre).HasColumnName("Per_Nombre");
                entity.Property(e => e.Per_Apellido).HasColumnName("Per_Apellido");
                entity.Property(e => e.TDoc_Nombre).HasColumnName("TDoc_Nombre");
                entity.Property(e => e.Per_Documento).HasColumnName("Per_Documento");
                entity.Property(e => e.Facturado).HasColumnName("Facturado");
            });

            modelBuilder.Entity<VwPersonaProductoCaro>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("View_PersonaProductoCaro");
                entity.Property(e => e.Per_Nombre).HasColumnName("Per_Nombre");
                entity.Property(e => e.Per_Apellido).HasColumnName("Per_Apellido");
                entity.Property(e => e.TDoc_Nombre).HasColumnName("TDoc_Nombre");
                entity.Property(e => e.Per_Documento).HasColumnName("Per_Documento");
                entity.Property(e => e.Producto).HasColumnName("Producto");
                entity.Property(e => e.Prod_Precio).HasColumnName("Prod_Precio");
            });

            modelBuilder.Entity<VwProductoCantidadFacturada>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("View_ProductoCantidadFacturada");
                entity.Property(e => e.Prod_ID).HasColumnName("Prod_ID");
                entity.Property(e => e.Prod_Descripcion).HasColumnName("Prod_Descripcion");
                entity.Property(e => e.Prod_Precio).HasColumnName("Prod_Precio");
                entity.Property(e => e.Prod_Costo).HasColumnName("Prod_Costo");
                entity.Property(e => e.Prod_UM).HasColumnName("Prod_UM");
                entity.Property(e => e.Cantidad).HasColumnName("Cantidad");
            });
            modelBuilder.Entity<VwProductoMargenGanaciaFacturada>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("View_ProductoMargenGanaciaFacturada");
                entity.Property(e => e.Prod_ID).HasColumnName("Prod_ID");
                entity.Property(e => e.Prod_Descripcion).HasColumnName("Prod_Descripcion");
                entity.Property(e => e.Prod_Precio).HasColumnName("Prod_Precio");
                entity.Property(e => e.Prod_Costo).HasColumnName("Prod_Costo");
                entity.Property(e => e.Prod_UM).HasColumnName("Prod_UM");
                entity.Property(e => e.MargenDeGanancia).HasColumnName("MargenDeGanancia");
            });
            modelBuilder.Entity<VwProductoUtilidadFacturada>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("View_ProductoUtilidadFacturada");
                entity.Property(e => e.Prod_ID).HasColumnName("Prod_ID");
                entity.Property(e => e.Prod_Descripcion).HasColumnName("Prod_Descripcion");
                entity.Property(e => e.Prod_Precio).HasColumnName("Prod_Precio");
                entity.Property(e => e.Prod_Costo).HasColumnName("Prod_Costo");
                entity.Property(e => e.Prod_UM).HasColumnName("Prod_UM");
                entity.Property(e => e.Utilidad).HasColumnName("Utilidad");
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
