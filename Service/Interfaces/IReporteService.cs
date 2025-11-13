using Domain.DTO;
using Domain.Models;
using Infraestructure;


namespace Service.Interfaces
{
    public interface IReporteService
    {
        void SetContext(MagnetronContext context);
        Response<IEnumerable<VwPersonaFacturado>> GetPersonaFacturado();
        Response<IEnumerable<VwPersonaProductoCaro>> GetPersonaProductoCaro();
        Response<IEnumerable<VwProductoCantidadFacturada>> GetProductoCantidadFacturada();
        Response<IEnumerable<VwProductoMargenGanaciaFacturada>> GetProductoMargenGanaciaFacturada();
        Response<IEnumerable<VwProductoUtilidadFacturada>> GetProductoUtilidadFacturada();
        
    }
}
