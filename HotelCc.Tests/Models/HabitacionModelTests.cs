using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelCc.Models;
using System;

namespace HotelCc.Tests.Models
{
    [TestClass]
    public class HabitacionModelTests
    {
        [TestMethod]
        [TestCategory("HabitacionModel")]
        public void Habitacion_Creation_WithValidData_ShouldSucceed()
        {
            // Arrange & Act
            var habitacion = new Habitacion
            {
                Id = 1,
                Numero = "101",
                Tipo = "Suite",
                Precio = 150.00m
            };

            // Assert
            Assert.IsNotNull(habitacion);
            Assert.AreEqual(1, habitacion.Id);
            Assert.AreEqual("101", habitacion.Numero);
            Assert.AreEqual("Suite", habitacion.Tipo);
            Assert.AreEqual(150.00m, habitacion.Precio);
        }

        [TestMethod]
        [TestCategory("HabitacionModel")]
        public void Habitacion_Numero_CanBeNull()
        {
            // Arrange & Act
            var habitacion = new Habitacion { Numero = null };

            // Assert
            Assert.IsNull(habitacion.Numero);
        }

        [TestMethod]
        [TestCategory("HabitacionModel")]
        public void Habitacion_Tipo_CanBeNull()
        {
            // Arrange & Act
            var habitacion = new Habitacion { Tipo = null };

            // Assert
            Assert.IsNull(habitacion.Tipo);
        }

        [TestMethod]
        [TestCategory("HabitacionModel")]
        public void Habitacion_Precio_DefaultValue_ShouldBeZero()
        {
            // Arrange & Act
            var habitacion = new Habitacion();

            // Assert
            Assert.AreEqual(0m, habitacion.Precio);
        }

        [TestMethod]
        [TestCategory("HabitacionModel")]
        public void Habitacion_Precio_CanBeUpdated()
        {
            // Arrange
            var habitacion = new Habitacion { Precio = 100.00m };

            // Act
            habitacion.Precio = 200.00m;

            // Assert
            Assert.AreEqual(200.00m, habitacion.Precio);
        }

        [TestMethod]
        [TestCategory("HabitacionModel")]
        public void Habitacion_Multiple_Instances_AreIndependent()
        {
            // Arrange
            var habitacion1 = new Habitacion { Id = 1, Numero = "101" };
            var habitacion2 = new Habitacion { Id = 2, Numero = "102" };

            // Act & Assert
            Assert.AreNotEqual(habitacion1.Id, habitacion2.Id);
            Assert.AreNotEqual(habitacion1.Numero, habitacion2.Numero);
        }

        [TestMethod]
        [TestCategory("HabitacionModel")]
        public void Habitacion_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var habitacion = new Habitacion();

            // Act
            habitacion.Id = 5;
            habitacion.Numero = "205";
            habitacion.Tipo = "Doble";
            habitacion.Precio = 180.50m;

            // Assert
            Assert.AreEqual(5, habitacion.Id);
            Assert.AreEqual("205", habitacion.Numero);
            Assert.AreEqual("Doble", habitacion.Tipo);
            Assert.AreEqual(180.50m, habitacion.Precio);
        }

        [TestMethod]
        [TestCategory("HabitacionModel")]
        public void Habitacion_Numero_StringFormatting()
        {
            // Arrange & Act
            var habitacion = new Habitacion { Numero = "101A" };

            // Assert
            Assert.AreEqual("101A", habitacion.Numero);
        }

        [TestMethod]
        [TestCategory("HabitacionModel")]
        public void Habitacion_Tipo_DifferentVariants()
        {
            // Arrange & Act
            var habitacion1 = new Habitacion { Tipo = "Suite" };
            var habitacion2 = new Habitacion { Tipo = "Doble" };
            var habitacion3 = new Habitacion { Tipo = "Simple" };

            // Assert
            Assert.AreEqual("Suite", habitacion1.Tipo);
            Assert.AreEqual("Doble", habitacion2.Tipo);
            Assert.AreEqual("Simple", habitacion3.Tipo);
        }

        [TestMethod]
        [TestCategory("HabitacionModel")]
        public void Habitacion_Precio_HighValue()
        {
            // Arrange & Act
            var habitacion = new Habitacion { Precio = 9999.99m };

            // Assert
            Assert.AreEqual(9999.99m, habitacion.Precio);
        }
    }
}
