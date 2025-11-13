using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Service.Interfaces;
using API.Controllers;
using Moq;
using Infraestructure;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.DTO;

namespace PruebasUnitarias
{
    [TestFixture]
    public class TestPersona
    {
        private MagnetronContext _context;
        private Mock<IPersonaService> _mockServicio;
        private PersonaController _controlador;
        private Mock<ILogger<PersonaController>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MagnetronContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;

            _context = new MagnetronContext(options);

            _context.Persona.Add(new Persona { Per_ID = 1, Per_Nombre = "Pepito", Per_Apellido = "Perez", Per_TipoDocumento = 1, Per_Documento = "123456" });
       
            _context.SaveChanges();

            _mockServicio = new Mock<IPersonaService>();
            _mockLogger = new Mock<ILogger<PersonaController>>();
            _controlador = new PersonaController(_mockServicio.Object, _context);
        }

        [Test]
        public void GetById()
        {
            
            var productoEsperado = new Response<PersonaDTO>() {
                Status = true,
                Message = "OK",
                Data = new PersonaDTO() {
                    Per_ID= 1,
                    Per_Nombre="Pepito",
                    Per_Apellido ="Perez",
                    Per_TipoDocumento= 1,
                    Per_Documento = "123456"
                }
            };
            _mockServicio.Setup(svc => svc.GetById(1)).Returns(productoEsperado);

            var resultado = _controlador.GetById(1);

            Assert.IsInstanceOf<Response<PersonaDTO>>(resultado);
            var okResult = resultado.Data as PersonaDTO;

            Assert.IsNotNull(okResult);

            Assert.AreEqual(1, okResult.Per_ID);
        }

        [Test]
        public void GetByIdNoData()
        {
            var productoEsperado = new Response<PersonaDTO>()
            {
                Status = false,
                Message = "The user dont exist",
                Data = null
            };
            _mockServicio.Setup(svc => svc.GetById(100)).Returns(productoEsperado);

            var resultado = _controlador.GetById(100);

            Assert.IsInstanceOf<Response<PersonaDTO>>(resultado);
            var okResult = resultado as Response<PersonaDTO>;

            Assert.IsNotNull(okResult);

            Assert.AreEqual(false, okResult.Status);

        }
    }
}