using Domain.DTO;
using Domain.Models;
using Infraestructure;
using Infraestructure.Repository;
using Service.Interfaces;
using Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ReporteService : IReporteService
    {
        private MagnetronContext _context;
        public void SetContext(MagnetronContext context) => _context = context;

        public Response<IEnumerable<VwPersonaFacturado>> GetPersonaFacturado()
        {
            var repository = new Repository<VwPersonaFacturado>(_context);
            return new Response<IEnumerable<VwPersonaFacturado>>()
            {
                Status = true,
                Message = "OK",
                Data = repository.Get()
            };
        }

        public Response<IEnumerable<VwPersonaProductoCaro>> GetPersonaProductoCaro()
        {
            var repository = new Repository<VwPersonaProductoCaro>(_context);
            return new Response<IEnumerable<VwPersonaProductoCaro>>()
            {
                Status = true,
                Message = "OK",
                Data = repository.Get()
            };
        }

        public Response<IEnumerable<VwProductoCantidadFacturada>> GetProductoCantidadFacturada()
        {
            var repository = new Repository<VwProductoCantidadFacturada>(_context);
            return new Response<IEnumerable<VwProductoCantidadFacturada>>()
            {
                Status = true,
                Message = "OK",
                Data = repository.Get().OrderByDescending(o => o.Cantidad)
            };
        }

        public Response<IEnumerable<VwProductoMargenGanaciaFacturada>> GetProductoMargenGanaciaFacturada()
        {
            var repository = new Repository<VwProductoMargenGanaciaFacturada>(_context);
            return new Response<IEnumerable<VwProductoMargenGanaciaFacturada>>()
            {
                Status = true,
                Message = "OK",
                Data = repository.Get()
            };
        }

        public Response<IEnumerable<VwProductoUtilidadFacturada>> GetProductoUtilidadFacturada()
        {
            var repository = new Repository<VwProductoUtilidadFacturada>(_context);
            return new Response<IEnumerable<VwProductoUtilidadFacturada>>()
            {
                Status = true,
                Message = "OK",
                Data = repository.Get()
            };
        }
    }
}
