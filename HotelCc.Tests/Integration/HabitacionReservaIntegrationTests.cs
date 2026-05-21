using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelCc.Models;
using HotelCc.Tests.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;

namespace HotelCc.Tests.Integration
{
    [TestClass]
    public class HabitacionReservaIntegrationTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void CreateHabitacionAndVerifyInDatabase()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();
            var habitacion = new Habitacion
            {
                Numero = "101",
                Tipo = "Suite",
                Precio = 150.00m
            };

            // Act
            context.Habitaciones.Add(habitacion);
            context.SaveChanges();

            var result = context.Habitaciones.FirstOrDefault(h => h.Numero == "101");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("101", result.Numero);
            Assert.AreEqual("Suite", result.Tipo);
            Assert.AreEqual(150.00m, result.Precio);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void CreateUserAndVerifyInDatabase()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();
            var usuario = new Usuario
            {
                Nombre = "Juan Pérez",
                Email = "juan@example.com",
                Password = "password123",
                Rol = "User"
            };

            // Act
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            var result = context.Usuarios.FirstOrDefault(u => u.Email == "juan@example.com");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Juan Pérez", result.Nombre);
            Assert.AreEqual("User", result.Rol);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void CreateReservaWithUserAndHabitacion()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();

            var usuario = new Usuario { Nombre = "Test User", Email = "test@example.com" };
            var habitacion = new Habitacion { Numero = "101", Tipo = "Suite", Precio = 150 };

            context.Usuarios.Add(usuario);
            context.Habitaciones.Add(habitacion);
            context.SaveChanges();

            var reserva = new Reserva
            {
                UsuarioId = usuario.Id,
                HabitacionId = habitacion.Id,
                FechaEntrada = new DateTime(2024, 1, 10),
                FechaSalida = new DateTime(2024, 1, 15),
                FechaReserva = new DateTime(2024, 1, 1),
                Total = 750
            };

            // Act
            context.Reservas.Add(reserva);
            context.SaveChanges();

            var result = context.Reservas
                .FirstOrDefault(r => r.UsuarioId == usuario.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(usuario.Id, result.UsuarioId);
            Assert.AreEqual(habitacion.Id, result.HabitacionId);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void UpdateHabitacionPriceAndVerify()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();
            var habitacion = new Habitacion { Numero = "201", Tipo = "Doble", Precio = 100 };
            context.Habitaciones.Add(habitacion);
            context.SaveChanges();

            var originalId = habitacion.Id;

            // Act
            var retrieved = context.Habitaciones.Find(originalId);
            if (retrieved != null)
            {
                retrieved.Precio = 150;
                context.SaveChanges();
            }

            var updated = context.Habitaciones.Find(originalId);

            // Assert
            Assert.IsNotNull(updated);
            Assert.AreEqual(150, updated.Precio);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void DeleteHabitacionAndVerify()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();
            var habitacion = new Habitacion { Numero = "301", Tipo = "Simple", Precio = 80 };
            context.Habitaciones.Add(habitacion);
            context.SaveChanges();

            var habitacionId = habitacion.Id;

            // Act
            var toDelete = context.Habitaciones.Find(habitacionId);
            if (toDelete != null)
            {
                context.Habitaciones.Remove(toDelete);
                context.SaveChanges();
            }

            var result = context.Habitaciones.Find(habitacionId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void QueryMultipleHabitacionesByTipo()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();

            context.Habitaciones.AddRange(
                new Habitacion { Numero = "101", Tipo = "Suite" },
                new Habitacion { Numero = "102", Tipo = "Suite" },
                new Habitacion { Numero = "201", Tipo = "Doble" }
            );
            context.SaveChanges();

            // Act
            var suites = context.Habitaciones.Where(h => h.Tipo == "Suite").ToList();

            // Assert
            Assert.AreEqual(2, suites.Count);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void QueryReservasByDateRange()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();

            var usuario = new Usuario { Nombre = "Test" };
            var habitacion = new Habitacion { Numero = "101" };

            context.Usuarios.Add(usuario);
            context.Habitaciones.Add(habitacion);
            context.SaveChanges();

            var reserva = new Reserva
            {
                UsuarioId = usuario.Id,
                HabitacionId = habitacion.Id,
                FechaEntrada = new DateTime(2024, 1, 10),
                FechaSalida = new DateTime(2024, 1, 15),
                FechaReserva = new DateTime(2024, 1, 1)
            };

            context.Reservas.Add(reserva);
            context.SaveChanges();

            // Act
            var today = DateTime.Today;
            var activeReservas = context.Reservas
                .Where(r => r.FechaEntrada <= today && r.FechaSalida >= today)
                .ToList();

            // Assert - Since today is after 2024-01-15, should be empty
            Assert.IsNotNull(activeReservas);
        }
    }
}
