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
    public class FacturaController : ControllerBase
    {
        private readonly IFacturaService _facturaService;
        private readonly ILogger<FacturaController> _logger;

        public FacturaController(ILogger<FacturaController> logger, IFacturaService facturaService, MagnetronContext context)
        {
            _logger = logger;
            _facturaService = facturaService;
            _facturaService.SetContext(context);
        }

        [HttpGet("GetAll", Name = "GetAllFactura")]
        public Response<IEnumerable<FacturaDTO>> GetAll()
        {
            return _facturaService.GetAll();
        }

        [HttpGet("GetById", Name = "GetByIdFactura")]
        public Response<FacturaDTO> GetById(int id)
        {
            return _facturaService.GetById(id);
        }

        [HttpPost("Add", Name = "AddFactura")]
        public Response<bool> Add(FacturaDTO factura)
        {
            return _facturaService.Add(factura);
        }

        [HttpDelete("Delete", Name = "DeleteFactura")]
        public Response<bool> Delete(int id)
        {
            return _facturaService.Delete(id);
        }

        [HttpPut("Update", Name = "UpdateFactura")]
        public Response<bool> Update(FacturaDTO factura)
        {
            return _facturaService.Update(factura);
        }
    }
}
