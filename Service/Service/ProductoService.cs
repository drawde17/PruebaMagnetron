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
    public class ProductoService : IProductoService
    {
        private MagnetronContext _context;
        public void SetContext(MagnetronContext context) => _context = context;

        public Response<IEnumerable<ProductoDTO>> GetAll()
        {
            try
            {
                var repository = new Repository<Producto>(_context);
                return new Response<IEnumerable<ProductoDTO>>()
                {
                    Status = true,
                    Message = "OK",
                    Data = repository.Get().Select(s => new ProductoDTO()
                    {
                        Prod_ID = s.Prod_ID,
                        Prod_Descripcion = s.Prod_Descripcion,
                        Prod_Precio = s.Prod_Precio,
                        Prod_Costo = s.Prod_Costo,
                        Prod_UM = s.Prod_UM
                    })
                };
            }
            catch (Exception ex)
            {
                var logError = new LogError<IEnumerable<ProductoDTO>>(_context);
                return logError.Set("ProductoService: getAll", ex);
            }
        }

        public Response<ProductoDTO> GetById(int id)
        {
            try
            {
                var repository = new Repository<Producto>(_context);
                var producto = repository.Get().FirstOrDefault(f => f.Prod_ID == id);
                if (producto == null)
                {
                    return new Response<ProductoDTO>()
                    {
                        Status = false,
                        Message = "The product dont exist"
                    };
                }

                return new Response<ProductoDTO>()
                {
                    Status = true,
                    Message = "OK",
                    Data = new ProductoDTO()
                    {
                        Prod_ID = producto.Prod_ID,
                        Prod_Descripcion = producto.Prod_Descripcion,
                        Prod_Precio= producto.Prod_Precio,
                        Prod_Costo = producto.Prod_Costo,
                        Prod_UM = producto.Prod_UM
                    }
                };
            }
            catch (Exception ex)
            {

                var logError = new LogError<ProductoDTO>(_context);
                return logError.Set("ProductoService: GetById", ex);
            }
        }

        public Response<bool> Add(ProductoDTO data)
        {
            try
            {
                var repository = new Repository<Producto>(_context);
                var producto = repository.Get();

                if (producto.Any(a => a.Prod_Descripcion == data.Prod_Descripcion))
                {
                    return new Response<bool>()
                    {
                        Status = false,
                        Message = "The producto already exist",
                        Data = false
                    };
                }

                var date = DateTime.Now;
                var productoToAdd = new Producto()
                {
                    Prod_ID = data.Prod_ID,
                    Prod_Descripcion= data.Prod_Descripcion,
                    Prod_Costo= data.Prod_Costo,
                    Prod_Precio = data.Prod_Precio,
                    Prod_UM = data.Prod_UM
                };
                repository.Add(productoToAdd);
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
                return logError.Set("ProductoService: add", ex);
            }
        }

        public Response<bool> Delete(int id)
        {
            try
            {
                var repository = new Repository<Producto>(_context);
                var producto = GetById(id);
                if (producto.Status == false)
                {
                    return new Response<bool>()
                    {
                        Status = false,
                        Message = producto.Message,
                        Data = false
                    };
                }

                var repositoryFactura = new Repository<FactDetalle>(_context);
                var productHasFactura = repositoryFactura.Get().FirstOrDefault(f => f.zProd_ID == id);
                if (productHasFactura != null)
                {
                    return new Response<bool>()
                    {
                        Status = false,
                        Message = "The product already has a Factura",
                        Data = false
                    };
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

        public Response<bool> Update(ProductoDTO data)
        {
            try
            {
                var repository = new Repository<Producto>(_context);
                var producto = repository.Get().FirstOrDefault(f => f.Prod_ID == data.Prod_ID);
                if (producto == null)
                {
                    return new Response<bool>()
                    {
                        Status = false,
                        Message = "The product already exist",
                        Data = false
                    };
                }
                var date = DateTime.Now;
                var productToUpdate = new Producto()
                {
                    Prod_ID = data.Prod_ID,
                    Prod_Descripcion = data.Prod_Descripcion,
                    Prod_Costo = data.Prod_Costo,
                    Prod_Precio = data.Prod_Precio,
                    Prod_UM = data.Prod_UM
                };
                repository.Update(productToUpdate);
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
    }
}
