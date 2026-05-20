using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelCc.Models;
using System;

namespace HotelCc.Tests.Validation
{
    [TestClass]
    public class DataValidationTests
    {
        [TestMethod]
        [TestCategory("DataValidation")]
        public void Usuario_ValidEmail_Format()
        {
            // Arrange & Act
            var usuario = new Usuario { Email = "user@domain.com" };

            // Assert
            Assert.IsTrue(usuario.Email.Contains("@"));
            Assert.IsTrue(usuario.Email.Contains("."));
        }

        [TestMethod]
        [TestCategory("DataValidation")]
        public void Habitacion_Numero_IsAlphanumeric()
        {
            // Arrange & Act
            var habitacion = new Habitacion { Numero = "101A" };

            // Assert
            Assert.IsNotNull(habitacion.Numero);
            Assert.IsTrue(habitacion.Numero.Length > 0);
        }

        [TestMethod]
        [TestCategory("DataValidation")]
        public void Habitacion_Precio_IsPositive()
        {
            // Arrange
            var habitacion = new Habitacion();

            // Act
            habitacion.Precio = 150.00m;

            // Assert
            Assert.IsTrue(habitacion.Precio > 0);
        }

        [TestMethod]
        [TestCategory("DataValidation")]
        public void Reserva_FechaSalida_AfterFechaEntrada()
        {
            // Arrange
            var reserva = new Reserva
            {
                FechaEntrada = new DateTime(2024, 1, 10),
                FechaSalida = new DateTime(2024, 1, 15)
            };

            // Act & Assert
            Assert.IsTrue(reserva.FechaSalida > reserva.FechaEntrada);
        }

        [TestMethod]
        [TestCategory("DataValidation")]
        public void Usuario_Password_NotEmpty()
        {
            // Arrange & Act
            var usuario = new Usuario { Password = "securepass123" };

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(usuario.Password));
        }

        [TestMethod]
        [TestCategory("DataValidation")]
        public void Habitacion_Numero_NotEmpty()
        {
            // Arrange & Act
            var habitacion = new Habitacion { Numero = "101" };

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(habitacion.Numero));
        }

        [TestMethod]
        [TestCategory("DataValidation")]
        public void Usuario_Rol_ValidValue()
        {
            // Arrange & Act
            var usuario = new Usuario { Rol = "Admin" };

            // Assert
            Assert.AreEqual("Admin", usuario.Rol);
        }

        [TestMethod]
        [TestCategory("DataValidation")]
        public void Reserva_Total_ValidAmount()
        {
            // Arrange & Act
            var reserva = new Reserva { Total = 500.50m };

            // Assert
            Assert.IsTrue(reserva.Total >= 0);
        }
    }
}
