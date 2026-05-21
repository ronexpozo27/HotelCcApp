using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelCc.Models;
using System;

namespace HotelCc.Tests.EdgeCases
{
    [TestClass]
    public class EdgeCaseTests
    {
        [TestMethod]
        [TestCategory("EdgeCases")]
        public void Habitacion_LongNumero_String()
        {
            // Arrange & Act
            var habitacion = new Habitacion { Numero = "12345ABCDE" };

            // Assert
            Assert.AreEqual("12345ABCDE", habitacion.Numero);
        }

        [TestMethod]
        [TestCategory("EdgeCases")]
        public void Usuario_LongEmail_String()
        {
            // Arrange & Act
            var usuario = new Usuario { Email = "very.long.email.address@subdomain.example.com" };

            // Assert
            Assert.IsTrue(usuario.Email.Contains("@"));
        }

        [TestMethod]
        [TestCategory("EdgeCases")]
        public void Habitacion_VeryHighPrice()
        {
            // Arrange & Act
            var habitacion = new Habitacion { Precio = 99999.99m };

            // Assert
            Assert.AreEqual(99999.99m, habitacion.Precio);
        }

        [TestMethod]
        [TestCategory("EdgeCases")]
        public void Habitacion_ZeroPrice()
        {
            // Arrange & Act
            var habitacion = new Habitacion { Precio = 0m };

            // Assert
            Assert.AreEqual(0m, habitacion.Precio);
        }

        [TestMethod]
        [TestCategory("EdgeCases")]
        public void Reserva_FutureDate()
        {
            // Arrange & Act
            var futureDate = DateTime.Now.AddYears(5);
            var reserva = new Reserva { FechaEntrada = futureDate };

            // Assert
            Assert.IsTrue(reserva.FechaEntrada > DateTime.Now);
        }

        [TestMethod]
        [TestCategory("EdgeCases")]
        public void Reserva_PastDate()
        {
            // Arrange & Act
            var pastDate = DateTime.Now.AddYears(-5);
            var reserva = new Reserva { FechaEntrada = pastDate };

            // Assert
            Assert.IsTrue(reserva.FechaEntrada < DateTime.Now);
        }

        [TestMethod]
        [TestCategory("EdgeCases")]
        public void Usuario_EmptyString_Email()
        {
            // Arrange & Act
            var usuario = new Usuario { Email = "" };

            // Assert
            Assert.AreEqual("", usuario.Email);
        }

        [TestMethod]
        [TestCategory("EdgeCases")]
        public void Habitacion_NegativePrice_IsAllowed()
        {
            // Arrange & Act
            var habitacion = new Habitacion { Precio = -100m };

            // Assert
            Assert.AreEqual(-100m, habitacion.Precio);
        }
    }
}
