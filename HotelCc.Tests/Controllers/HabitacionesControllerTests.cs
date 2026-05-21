using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using HotelCc.Controllers;
using HotelCc.Models;
using HotelCc.Tests.Helpers;
using System;
using System.Threading.Tasks;

namespace HotelCc.Tests.Controllers
{
    [TestClass]
    public class HabitacionesControllerTests
    {
        private HabitacionesController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            var context = TestDatabaseHelper.CreateTestContext();
            _controller = new HabitacionesController(context);
        }

        [TestMethod]
        [TestCategory("HabitacionesController")]
        public async Task Index_ReturnsViewResult()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [TestCategory("HabitacionesController")]
        public async Task Details_WithNullId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        [TestCategory("HabitacionesController")]
        public async Task Details_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Details(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        [TestCategory("HabitacionesController")]
        public void Create_Get_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [TestCategory("HabitacionesController")]
        public async Task Create_Post_WithValidData_RedirectsToIndex()
        {
            // Arrange
            var habitacion = new Habitacion
            {
                Numero = "101",
                Tipo = "Suite",
                Precio = 150
            };

            // Act
            var result = await _controller.Create(habitacion);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [TestMethod]
        [TestCategory("HabitacionesController")]
        public async Task Create_Post_WithInvalidData_ReturnsView()
        {
            // Arrange
            _controller.ModelState.AddModelError("Numero", "Required");
            var habitacion = new Habitacion { Numero = "", Tipo = "Suite" };

            // Act
            var result = await _controller.Create(habitacion);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [TestCategory("HabitacionesController")]
        public void Create_Get_ReturnedViewIsNotNull()
        {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
