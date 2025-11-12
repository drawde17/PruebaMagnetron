using Domain.DTO;
using Domain.Models;
using Infraestructure;


namespace Service.Interfaces
{
    public interface IPersonaService
    {
        void SetContext(MagnetronContext context);
        Response<IEnumerable<PersonaDTO>> GetAll();
        Response<bool> Add(PersonaDTO data);
        Response<PersonaDTO> GetById(int id);
        Response<bool> Delete(int id);
        Response<bool> Update(PersonaDTO data);
    }
}
