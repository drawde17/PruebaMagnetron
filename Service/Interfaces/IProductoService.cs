using Domain.DTO;
using Domain.Models;
using Infraestructure;


namespace Service.Interfaces
{
    public interface IProductoService
    {
        void SetContext(MagnetronContext context);
        Response<IEnumerable<ProductoDTO>> GetAll();
        Response<bool> Add(ProductoDTO data);
        Response<ProductoDTO> GetById(int id);
        Response<bool> Delete(int id);
        Response<bool> Update(ProductoDTO data);
    }
}
