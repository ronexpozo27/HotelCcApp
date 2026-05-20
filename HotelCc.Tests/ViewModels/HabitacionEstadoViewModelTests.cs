using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelCc.Models;
using System;

namespace HotelCc.Tests.ViewModels
{
    [TestClass]
    public class HabitacionEstadoViewModelTests
    {
        [TestMethod]
        [TestCategory("HabitacionEstadoViewModel")]
        public void HabitacionEstadoViewModel_Creation_WithValidData()
        {
            // Arrange & Act
            var model = new HabitacionEstadoViewModel
            {
                Id = 1,
                Numero = "101",
                Tipo = "Suite",
                Precio = 150.00m,
                Estado = "Disponible"
            };

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("101", model.Numero);
            Assert.AreEqual("Suite", model.Tipo);
            Assert.AreEqual(150.00m, model.Precio);
            Assert.AreEqual("Disponible", model.Estado);
        }

        [TestMethod]
        [TestCategory("HabitacionEstadoViewModel")]
        public void HabitacionEstadoViewModel_Estado_Ocupada()
        {
            // Arrange & Act
            var model = new HabitacionEstadoViewModel { Estado = "Ocupada" };

            // Assert
            Assert.AreEqual("Ocupada", model.Estado);
        }

        [TestMethod]
        [TestCategory("HabitacionEstadoViewModel")]
        public void HabitacionEstadoViewModel_Estado_Disponible()
        {
            // Arrange & Act
            var model = new HabitacionEstadoViewModel { Estado = "Disponible" };

            // Assert
            Assert.AreEqual("Disponible", model.Estado);
        }

        [TestMethod]
        [TestCategory("HabitacionEstadoViewModel")]
        public void HabitacionEstadoViewModel_Properties_CanBeUpdated()
        {
            // Arrange
            var model = new HabitacionEstadoViewModel
            {
                Id = 1,
                Numero = "101",
                Tipo = "Suite",
                Precio = 150.00m,
                Estado = "Disponible"
            };

            // Act
            model.Estado = "Ocupada";
            model.Precio = 200.00m;

            // Assert
            Assert.AreEqual("Ocupada", model.Estado);
            Assert.AreEqual(200.00m, model.Precio);
        }

        [TestMethod]
        [TestCategory("HabitacionEstadoViewModel")]
        public void HabitacionEstadoViewModel_Multiple_Instances_AreIndependent()
        {
            // Arrange
            var model1 = new HabitacionEstadoViewModel { Id = 1, Numero = "101" };
            var model2 = new HabitacionEstadoViewModel { Id = 2, Numero = "102" };

            // Act & Assert
            Assert.AreNotEqual(model1.Id, model2.Id);
            Assert.AreNotEqual(model1.Numero, model2.Numero);
        }

        [TestMethod]
        [TestCategory("HabitacionEstadoViewModel")]
        public void HabitacionEstadoViewModel_Numero_StringValue()
        {
            // Arrange & Act
            var model = new HabitacionEstadoViewModel { Numero = "501" };

            // Assert
            Assert.AreEqual("501", model.Numero);
        }

        [TestMethod]
        [TestCategory("HabitacionEstadoViewModel")]
        public void HabitacionEstadoViewModel_Tipo_Doble()
        {
            // Arrange & Act
            var model = new HabitacionEstadoViewModel { Tipo = "Doble" };

            // Assert
            Assert.AreEqual("Doble", model.Tipo);
        }

        [TestMethod]
        [TestCategory("HabitacionEstadoViewModel")]
        public void HabitacionEstadoViewModel_Precio_DecimalValue()
        {
            // Arrange & Act
            var model = new HabitacionEstadoViewModel { Precio = 250.50m };

            // Assert
            Assert.AreEqual(250.50m, model.Precio);
        }
    }
}
