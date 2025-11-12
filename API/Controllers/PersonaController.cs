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
    public class PersonaController : ControllerBase
    {
        private readonly IPersonaService _personaService;
        private readonly ILogger<PersonaController> _logger;

        public PersonaController(ILogger<PersonaController> logger, IPersonaService personaService, MagnetronContext context)
        {
            _logger = logger;
            _personaService = personaService;
            _personaService.SetContext(context);
        }

        [HttpGet("GetAll", Name = "GetAllPersons")]
        public Response<IEnumerable<PersonaDTO>> GetAll()
        {
            return _personaService.GetAll();
        }

        [HttpGet("GetById", Name = "GetByIdPerson")]
        public Response<PersonaDTO> GetById(int id)
        {
            return _personaService.GetById(id);
        }

        [AllowAnonymous]
        [HttpPost("Add", Name = "AddPerson")]
        public Response<bool> Add(PersonaDTO user)
        {
            return _personaService.Add(user);
        }

        [HttpDelete("Delete", Name = "DeletePerson")]
        public Response<bool> Delete(int id)
        {
            return _personaService.Delete(id);
        }

        [HttpPut("Update", Name = "UpdatePerson")]
        public Response<bool> Update(PersonaDTO user)
        {
            return _personaService.Update(user);
        }
    }
}
