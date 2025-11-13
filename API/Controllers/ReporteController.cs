using Domain.DTO;
using Domain.Models;
using Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.Interfaces;
using Service.Service;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReporteController : ControllerBase
    {
        private readonly IReporteService _reporteService;
        private readonly ILogger<ReporteController> _logger;

        public ReporteController(ILogger<ReporteController> logger, IReporteService reporteService, MagnetronContext context)
        {
            _logger = logger;
            _reporteService = reporteService;
            _reporteService.SetContext(context);
        }

        [HttpGet("PersonaFacturado", Name = "GetPersonaFacturado")]
        public Response<IEnumerable<VwPersonaFacturado>> PersonaFacturado()
        {
            return _reporteService.GetPersonaFacturado();
        }

        [HttpGet("PersonaProductoCaro", Name = "GetPersonaProductoCaro")]
        public Response<IEnumerable<VwPersonaProductoCaro>> PersonaProductoCaro()
        {
            return _reporteService.GetPersonaProductoCaro();
        }

        [HttpGet("ProductoCantidadFacturada", Name = "GetProductoCantidadFacturada")]
        public Response<IEnumerable<VwProductoCantidadFacturada>> ProductoCantidadFacturada()
        {
            return _reporteService.GetProductoCantidadFacturada();
        }

        [HttpGet("ProductoMargenGanaciaFacturada", Name = "GetProductoMargenGanaciaFacturada")]
        public Response<IEnumerable<VwProductoMargenGanaciaFacturada>> ProductoMargenGanaciaFacturada()
        {
            return _reporteService.GetProductoMargenGanaciaFacturada();
        }

        [HttpGet("ProductoUtilidadFacturada", Name = "GetProductoUtilidadFacturada")]
        public Response<IEnumerable<VwProductoUtilidadFacturada>> ProductoUtilidadFacturada()
        {
            return _reporteService.GetProductoUtilidadFacturada();
        }


    }
}
