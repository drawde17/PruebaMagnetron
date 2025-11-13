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
    public class FacturaService : IFacturaService
    {
        private MagnetronContext _context;
        public void SetContext(MagnetronContext context) => _context = context;

        public Response<IEnumerable<FacturaDTO>> GetAll()
        {
            try
            {
                var repositoryFEncabezado = new Repository<FactEncabezado>(_context);
                
                return new Response<IEnumerable<FacturaDTO>>()
                {
                    Status = true,
                    Message = "OK",
                    Data = repositoryFEncabezado.Get().Select(s => new FacturaDTO()
                    {
                        FEnc_ID = s.FEnc_ID,
                        FEnc_Numero = s.FEnc_Numero,
                        FEnc_Fecha = s.FEnc_Fecha,
                        zPer_ID = s.zPer_ID,
                        FacturaDetalle = getFacturaByEncId(s.FEnc_ID)
                    })
                };
            }
            catch (Exception ex)
            {
                var logError = new LogError<IEnumerable<FacturaDTO>>(_context);
                return logError.Set("FacturaService: getAll", ex);
            }
        }

        private IEnumerable<FacturaDetalleDTO> getFacturaByEncId(int FEnc_ID) 
        {
            try
            {
                var repositoryFDetalle = new Repository<FactDetalle>(_context);
                return repositoryFDetalle.GetWithFilters(f => f.zFEnc_ID == FEnc_ID)
                    .Select(s2 => new FacturaDetalleDTO()
                    {
                        FDet_ID = s2.FDet_ID,
                        FDet_Linea = s2.FDet_Linea,
                        FDet_Cantidad = s2.FDet_Cantidad,
                        zProd_ID = s2.zProd_ID,
                        zFEnc_ID = s2.zFEnc_ID
                    });
            }
            catch (Exception ex)
            {
                var logError = new LogError<IEnumerable<FacturaDTO>>(_context);
                logError.Set("FacturaService: getFacturaByEncId", ex);
                return new List<FacturaDetalleDTO>();
            }
        }

        public Response<FacturaDTO> GetById(int id)
        {
            try
            {
                var repository = new Repository<FactEncabezado>(_context);
                var factura = repository.Get().FirstOrDefault(f => f.FEnc_ID == id);
                if (factura == null)
                {
                    return new Response<FacturaDTO>()
                    {
                        Status = false,
                        Message = "The factura dont exist"
                    };
                }

                return new Response<FacturaDTO>()
                {
                    Status = true,
                    Message = "OK",
                    Data = new FacturaDTO()
                    {
                        FEnc_ID = factura.FEnc_ID,
                        FEnc_Numero = factura.FEnc_Numero,
                        FEnc_Fecha = factura.FEnc_Fecha,
                        zPer_ID = factura.zPer_ID,
                        FacturaDetalle = getFacturaByEncId(factura.FEnc_ID)
                    }
                };
            }
            catch (Exception ex)
            {

                var logError = new LogError<FacturaDTO>(_context);
                return logError.Set("FacturaService: GetById", ex);
            }
        }

        public Response<bool> Add(FacturaDTO data)
        {
            try
            {
                var repository = new Repository<FactEncabezado>(_context);
                var fDetalleRepository = new Repository<FactDetalle>(_context);
                var factura = repository.Get();

                if (factura.Any(a => a.FEnc_ID == data.FEnc_ID))
                {
                    return new Response<bool>()
                    {
                        Status = false,
                        Message = "The factura already exist",
                        Data = false
                    };
                }

                var personaRepository = new Repository<Persona>(_context);
                var persona = personaRepository.Get();
                if (!persona.Any(a => a.Per_ID == data.zPer_ID)) 
                {
                    return new Response<bool>()
                    {
                        Status = false,
                        Message = "The persona dont exist",
                        Data = false
                    };
                }

                var date = DateTime.Now;
                var facturaToAdd = new FactEncabezado()
                {
                    FEnc_ID = data.FEnc_ID,
                    FEnc_Numero = data.FEnc_Numero,
                    FEnc_Fecha= date,
                    zPer_ID = data.zPer_ID
                };
                repository.Add(facturaToAdd);
                repository.Save();

                foreach (var detalle in data.FacturaDetalle)
                {
                    var productoRepository = new Repository<Producto>(_context);
                    var producto = productoRepository.Get();
                    if (producto.Any(a => a.Prod_ID == detalle.zProd_ID))
                    {
                        var facturaDetalleToAdd = new FactDetalle()
                        {
                            FDet_Linea = detalle.FDet_Linea,
                            FDet_Cantidad = detalle.FDet_Cantidad,
                            zFEnc_ID = facturaToAdd.FEnc_ID,
                            zProd_ID = detalle.zProd_ID
                        };
                        fDetalleRepository.Add(facturaDetalleToAdd);
                        repository.Save();
                    }
                }

                return new Response<bool>()
                {
                    Status = true,
                    Message = "OK",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                var logError = new LogError<bool>(_context);
                return logError.Set("FacturaService: add", ex);
            }
        }

        public Response<bool> Delete(int id)
        {
            try
            {
                var repository = new Repository<FactEncabezado>(_context);
                var factura = GetById(id);
                if (factura.Status == false)
                {
                    return new Response<bool>()
                    {
                        Status = false,
                        Message = factura.Message,
                        Data = false
                    };
                }

                var fDetalleRepository = new Repository<FactDetalle>(_context);
                var facturaDetails = fDetalleRepository.GetWithFilters(f => f.zFEnc_ID == id);

                foreach (var detalle in facturaDetails) 
                {
                    fDetalleRepository.Delete(detalle.FDet_ID);
                    fDetalleRepository.Save();
                }

                repository.Delete(id);
                repository.Save();
                return new Response<bool>()
                {
                    Status = true,
                    Message = "OK",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                var logError = new LogError<bool>(_context);
                return logError.Set("ProductoService: Delete", ex);
            }
        }

        public Response<bool> Update(FacturaDTO data)
        {
            try
            {
                var repository = new Repository<FactEncabezado>(_context);
                var factura = repository.Get().FirstOrDefault(f => f.FEnc_ID == data.FEnc_ID);
                if (factura == null)
                {
                    return new Response<bool>()
                    {
                        Status = false,
                        Message = "The factura dont exist",
                        Data = false
                    };
                }
                var date = DateTime.Now;
                var facturaToUpdate = new FactEncabezado()
                {
                    FEnc_ID = data.FEnc_ID,
                    FEnc_Numero = data.FEnc_Numero,
                    FEnc_Fecha = data.FEnc_Fecha,
                    zPer_ID = data.zPer_ID
                };
                repository.Update(facturaToUpdate);
                repository.Save();

                var fDetalleRepository = new Repository<FactDetalle>(_context);
                var facturaDetails = fDetalleRepository.GetWithFilters(f => f.zFEnc_ID == data.FEnc_ID);

                var detalleToDelete = facturaDetails.Where(w => !data.FacturaDetalle.Any(a => a.FDet_ID == w.FDet_ID));

                foreach (var detalle in detalleToDelete) 
                {
                    fDetalleRepository.Delete(detalle.FDet_ID);
                    fDetalleRepository.Save();
                }

                foreach (var detalle in data.FacturaDetalle)
                {
                    var existDetalleBD = facturaDetails.FirstOrDefault(f => f.FDet_ID == detalle.FDet_ID);
                    var existDetalleData = facturaDetails.FirstOrDefault(f => f.FDet_ID == detalle.FDet_ID);

                    var facturaDetalleToBd = new FactDetalle()
                    {
                        FDet_Linea = detalle.FDet_Linea,
                        FDet_Cantidad = detalle.FDet_Cantidad,
                        zFEnc_ID = data.FEnc_ID,
                        zProd_ID = detalle.zProd_ID
                    };

                    if (existDetalleBD == null)
                    {
                        fDetalleRepository.Add(facturaDetalleToBd);
                        fDetalleRepository.Save();
                    }
                    else
                    {
                        facturaDetalleToBd.FDet_ID = detalle.FDet_ID;
                        fDetalleRepository.Update(facturaDetalleToBd);
                        fDetalleRepository.Save();
                    }
                }

                return new Response<bool>()
                {
                    Status = true,
                    Message = "OK",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                var logError = new LogError<bool>(_context);
                return logError.Set("ProductoService: Delete", ex);
            }
        }
    }
}
