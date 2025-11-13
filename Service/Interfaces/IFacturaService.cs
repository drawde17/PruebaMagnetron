using Domain.DTO;
using Domain.Models;
using Infraestructure;


namespace Service.Interfaces
{
    public interface IFacturaService
    {
        void SetContext(MagnetronContext context);
        Response<IEnumerable<FacturaDTO>> GetAll();
        Response<bool> Add(FacturaDTO data);
        Response<FacturaDTO> GetById(int id);
        Response<bool> Delete(int id);
        Response<bool> Update(FacturaDTO data);
    }
}
