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
    public class PersonaService : IPersonaService
    {
        private MagnetronContext _context;
        public void SetContext(MagnetronContext context) => _context = context;

        public Response<IEnumerable<PersonaDTO>> GetAll()
        {
            try
            {
                var repository = new Repository<Persona>(_context);
                return new Response<IEnumerable<PersonaDTO>>()
                {
                    Status = true,
                    Message = "OK",
                    Data = repository.Get().Select(s => new PersonaDTO()
                    {
                        Per_ID = s.Per_ID,
                        Per_Nombre = s.Per_Nombre,
                        Per_Apellido = s.Per_Apellido,
                        Per_TipoDocumento = s.Per_TipoDocumento,
                        Per_Documento = s.Per_Documento
                    })
                };
            }
            catch (Exception ex)
            {
                var logError = new LogError<IEnumerable<PersonaDTO>>(_context);
                return logError.Set("PersonaService: getAll", ex);
            }
        }

        public Response<PersonaDTO> GetById(int id)
        {
            try
            {
                var repository = new Repository<Persona>(_context);
                var persona = repository.Get().FirstOrDefault(f => f.Per_ID == id);
                if (persona == null)
                {
                    return new Response<PersonaDTO>()
                    {
                        Status = false,
                        Message = "The user dont exist"
                    };
                }

                return new Response<PersonaDTO>()
                {
                    Status = true,
                    Message = "OK",
                    Data = new PersonaDTO()
                    {
                        Per_ID = persona.Per_ID,
                        Per_Nombre = persona.Per_Nombre,
                        Per_Apellido = persona.Per_Apellido,
                        Per_TipoDocumento = persona.Per_TipoDocumento,
                        Per_Documento = persona.Per_Documento
                    }
                };
            }
            catch (Exception ex)
            {

                var logError = new LogError<PersonaDTO>(_context);
                return logError.Set("PersonaService: GetById", ex);
            }
        }

        public Response<bool> Add(PersonaDTO data)
        {
            try
            {
                var repository = new Repository<Persona>(_context);
                var persona = repository.Get();

                if (persona.Any(a => a.Per_TipoDocumento == data.Per_TipoDocumento && a.Per_Documento == data.Per_Documento))
                {
                    return new Response<bool>()
                    {
                        Status = false,
                        Message = "The user already exist",
                        Data = false
                    };
                }

                var date = DateTime.Now;
                var personaToAdd = new Persona()
                {
                    Per_ID= data.Per_ID,
                    Per_Nombre= data.Per_Nombre,
                    Per_Apellido= data.Per_Apellido,
                    Per_TipoDocumento= data.Per_TipoDocumento,
                    Per_Documento = data.Per_Documento
                };
                repository.Add(personaToAdd);
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
                return logError.Set("PersonaService: add", ex);
            }
        }

        public Response<bool> Delete(int id)
        {
            try
            {
                var repository = new Repository<Persona>(_context);
                var persona = GetById(id);
                if (persona.Status == false)
                {
                    return new Response<bool>()
                    {
                        Status = false,
                        Message = persona.Message,
                        Data = false
                    };
                }

                var repositoryFactura = new Repository<FactEncabezado>(_context);
                var personHasFactura = repositoryFactura.Get().FirstOrDefault(f => f.zPer_ID == id);
                if (personHasFactura != null)
                {
                    return new Response<bool>()
                    {
                        Status = false,
                        Message = "The persona already has a Factura",
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
                return logError.Set("PersonaService: Delete", ex);
            }
        }

        public Response<bool> Update(PersonaDTO data)
        {
            try
            {
                var repository = new Repository<Persona>(_context);
                var persona = repository.Get().FirstOrDefault(f => f.Per_ID == data.Per_ID);
                if (persona == null)
                {
                    return new Response<bool>()
                    {
                        Status = false,
                        Message = "The persona already exist",
                        Data = false
                    };
                }
                var date = DateTime.Now;
                var personaToUpdate = new Persona()
                {
                    Per_ID = data.Per_ID,
                    Per_Nombre = data.Per_Nombre,
                    Per_Apellido = data.Per_Apellido,
                    Per_TipoDocumento = data.Per_TipoDocumento,
                    Per_Documento = data.Per_Documento
                };
                repository.Update(personaToUpdate);
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
                return logError.Set("PersonaService: Delete", ex);
            }
        }
    }
}
