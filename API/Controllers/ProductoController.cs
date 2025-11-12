using Domain.DTO;
using Domain.Models;
using Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(ILogger<ProductoController> logger, IProductoService productoService, MagnetronContext context)
        {
            _logger = logger;
            _productoService = productoService;
            _productoService.SetContext(context);
        }

        [HttpGet("GetAll", Name = "GetAllProduct")]
        public Response<IEnumerable<ProductoDTO>> GetAll()
        {
            return _productoService.GetAll();
        }

        [HttpGet("GetById", Name = "GetByIdProduct")]
        public Response<ProductoDTO> GetById(int id)
        {
            return _productoService.GetById(id);
        }

        [AllowAnonymous]
        [HttpPost("Add", Name = "AddProduct")]
        public Response<bool> Add(ProductoDTO producto)
        {
            return _productoService.Add(producto);
        }

        [HttpDelete("Delete", Name = "DeleteProduct")]
        public Response<bool> Delete(int id)
        {
            return _productoService.Delete(id);
        }

        [HttpPut("Update", Name = "UpdateProduct")]
        public Response<bool> Update(ProductoDTO producto)
        {
            return _productoService.Update(producto);
        }
    }
}
