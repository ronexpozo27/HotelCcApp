using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelCc.Models;
using System;

namespace HotelCc.Tests.Models
{
    [TestClass]
    public class ReservaModelTests
    {
        [TestMethod]
        [TestCategory("ReservaModel")]
        public void Reserva_Creation_WithValidData_ShouldSucceed()
        {
            // Arrange & Act
            var reserva = new Reserva
            {
                Id = 1,
                UsuarioId = 1,
                HabitacionId = 1,
                FechaEntrada = new DateTime(2024, 1, 10),
                FechaSalida = new DateTime(2024, 1, 15),
                FechaReserva = new DateTime(2024, 1, 1),
                Total = 750.00m
            };

            // Assert
            Assert.IsNotNull(reserva);
            Assert.AreEqual(1, reserva.Id);
            Assert.AreEqual(1, reserva.UsuarioId);
            Assert.AreEqual(1, reserva.HabitacionId);
            Assert.AreEqual(750.00m, reserva.Total);
        }

        [TestMethod]
        [TestCategory("ReservaModel")]
        public void Reserva_Usuario_CanBeNull()
        {
            // Arrange & Act
            var reserva = new Reserva { Usuario = null };

            // Assert
            Assert.IsNull(reserva.Usuario);
        }

        [TestMethod]
        [TestCategory("ReservaModel")]
        public void Reserva_Habitacion_CanBeNull()
        {
            // Arrange & Act
            var reserva = new Reserva { Habitacion = null };

            // Assert
            Assert.IsNull(reserva.Habitacion);
        }

        [TestMethod]
        [TestCategory("ReservaModel")]
        public void Reserva_FechaEntrada_CanBeSet()
        {
            // Arrange
            var fecha = new DateTime(2024, 2, 20);

            // Act
            var reserva = new Reserva { FechaEntrada = fecha };

            // Assert
            Assert.AreEqual(fecha, reserva.FechaEntrada);
        }

        [TestMethod]
        [TestCategory("ReservaModel")]
        public void Reserva_FechaSalida_CanBeSet()
        {
            // Arrange
            var fecha = new DateTime(2024, 2, 25);

            // Act
            var reserva = new Reserva { FechaSalida = fecha };

            // Assert
            Assert.AreEqual(fecha, reserva.FechaSalida);
        }

        [TestMethod]
        [TestCategory("ReservaModel")]
        public void Reserva_FechaReserva_CanBeSet()
        {
            // Arrange
            var fecha = new DateTime(2024, 2, 1);

            // Act
            var reserva = new Reserva { FechaReserva = fecha };

            // Assert
            Assert.AreEqual(fecha, reserva.FechaReserva);
        }

        [TestMethod]
        [TestCategory("ReservaModel")]
        public void Reserva_Total_CanBeUpdated()
        {
            // Arrange
            var reserva = new Reserva { Total = 500.00m };

            // Act
            reserva.Total = 1000.00m;

            // Assert
            Assert.AreEqual(1000.00m, reserva.Total);
        }

        [TestMethod]
        [TestCategory("ReservaModel")]
        public void Reserva_Multiple_Instances_AreIndependent()
        {
            // Arrange
            var reserva1 = new Reserva { Id = 1, UsuarioId = 1 };
            var reserva2 = new Reserva { Id = 2, UsuarioId = 2 };

            // Act & Assert
            Assert.AreNotEqual(reserva1.Id, reserva2.Id);
            Assert.AreNotEqual(reserva1.UsuarioId, reserva2.UsuarioId);
        }

        [TestMethod]
        [TestCategory("ReservaModel")]
        public void Reserva_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var reserva = new Reserva();

            // Act
            reserva.Id = 5;
            reserva.UsuarioId = 3;
            reserva.HabitacionId = 2;
            reserva.FechaEntrada = new DateTime(2024, 3, 10);
            reserva.FechaSalida = new DateTime(2024, 3, 15);
            reserva.FechaReserva = new DateTime(2024, 2, 28);
            reserva.Total = 600.00m;

            // Assert
            Assert.AreEqual(5, reserva.Id);
            Assert.AreEqual(3, reserva.UsuarioId);
            Assert.AreEqual(2, reserva.HabitacionId);
            Assert.AreEqual(new DateTime(2024, 3, 10), reserva.FechaEntrada);
            Assert.AreEqual(new DateTime(2024, 3, 15), reserva.FechaSalida);
            Assert.AreEqual(new DateTime(2024, 2, 28), reserva.FechaReserva);
            Assert.AreEqual(600.00m, reserva.Total);
        }

        [TestMethod]
        [TestCategory("ReservaModel")]
        public void Reserva_Total_DefaultValue()
        {
            // Arrange & Act
            var reserva = new Reserva();

            // Assert
            Assert.AreEqual(0m, reserva.Total);
        }
    }
}
