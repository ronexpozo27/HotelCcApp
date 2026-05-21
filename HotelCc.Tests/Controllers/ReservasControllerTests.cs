using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using HotelCc.Controllers;
using HotelCc.Models;
using HotelCc.Data;
using HotelCc.Tests.Helpers;
using System;
using System.Threading.Tasks;

namespace HotelCc.Tests.Controllers
{
    [TestClass]
    public class ReservasControllerTests
    {
        private ReservasController _controller = null!;
        private AppDbContext _context = null!;

        [TestInitialize]
        public void Setup()
        {
            _context = TestDatabaseHelper.CreateTestContext();
            _controller = new ReservasController(_context);
            HttpContextMockHelper.SetupHttpContextWithSession(_controller);
        }

        [TestMethod]
        [TestCategory("ReservasController")]
        public async Task Index_ReturnsViewResult()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [TestCategory("ReservasController")]
        public async Task Details_WithNullId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        [TestCategory("ReservasController")]
        public async Task Details_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Details(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        [TestCategory("ReservasController")]
        public void Create_Get_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [TestCategory("ReservasController")]
        public void Create_Get_ReturnedViewIsNotNull()
        {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("ReservasController")]
        public void Create_Get_ContainsHabitacionesSelectList()
        {
            // Arrange
            var context = TestDatabaseHelper.CreateTestContext();
            var habitacion = new Habitacion { Numero = "101", Tipo = "Suite" };
            context.Habitaciones.Add(habitacion);
            context.SaveChanges();

            var controller = new ReservasController(context);

            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewData["HabitacionId"]);
        }
    }
}
