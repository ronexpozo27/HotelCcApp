using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelCc.Models;
using HotelCc.Tests.Helpers;
using System;
using System.Linq;

namespace HotelCc.Tests.Data
{
    [TestClass]
    public class AppDbContextTests
    {
        [TestMethod]
        [TestCategory("AppDbContext")]
        public void AppDbContext_Usuarios_CanAddUser()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();
            var usuario = new Usuario { Nombre = "Test User", Email = "test@example.com" };

            // Act
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(1, context.Usuarios.Count());
        }

        [TestMethod]
        [TestCategory("AppDbContext")]
        public void AppDbContext_Habitaciones_CanAddRoom()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();
            var habitacion = new Habitacion { Numero = "101", Tipo = "Suite" };

            // Act
            context.Habitaciones.Add(habitacion);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(1, context.Habitaciones.Count());
        }

        [TestMethod]
        [TestCategory("AppDbContext")]
        public void AppDbContext_Reservas_CanAddReservation()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();
            var reserva = new Reserva { UsuarioId = 1, HabitacionId = 1, Total = 500 };

            // Act
            context.Reservas.Add(reserva);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(1, context.Reservas.Count());
        }

        [TestMethod]
        [TestCategory("AppDbContext")]
        public void AppDbContext_CanAddMultipleUsers()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();

            // Act
            for (int i = 1; i <= 5; i++)
            {
                context.Usuarios.Add(new Usuario { Nombre = $"User{i}" });
            }
            context.SaveChanges();

            // Assert
            Assert.AreEqual(5, context.Usuarios.Count());
        }

        [TestMethod]
        [TestCategory("AppDbContext")]
        public void AppDbContext_CanAddMultipleRooms()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();

            // Act
            for (int i = 1; i <= 10; i++)
            {
                context.Habitaciones.Add(new Habitacion { Numero = $"10{i}" });
            }
            context.SaveChanges();

            // Assert
            Assert.AreEqual(10, context.Habitaciones.Count());
        }

        [TestMethod]
        [TestCategory("AppDbContext")]
        public void AppDbContext_CanQueryUser()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();
            var usuario = new Usuario { Nombre = "John Doe", Email = "john@example.com" };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            // Act
            var result = context.Usuarios.FirstOrDefault(u => u.Email == "john@example.com");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("John Doe", result.Nombre);
        }
    }
}
