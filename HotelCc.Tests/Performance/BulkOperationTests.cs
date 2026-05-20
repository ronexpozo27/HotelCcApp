using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelCc.Models;
using HotelCc.Tests.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;

namespace HotelCc.Tests.Performance
{
    [TestClass]
    public class BulkOperationTests
    {
        [TestMethod]
        [TestCategory("Performance")]
        public void AddMultipleHabitaciones_BulkInsert()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();
            var habitaciones = new List<Habitacion>();

            for (int i = 1; i <= 20; i++)
            {
                habitaciones.Add(new Habitacion
                {
                    Numero = $"{i:D3}",
                    Tipo = i % 3 == 0 ? "Suite" : i % 2 == 0 ? "Doble" : "Simple",
                    Precio = 100 + (i * 10)
                });
            }

            // Act
            context.Habitaciones.AddRange(habitaciones);
            context.SaveChanges();

            var count = context.Habitaciones.Count();

            // Assert
            Assert.AreEqual(20, count);
        }

        [TestMethod]
        [TestCategory("Performance")]
        public void AddMultipleUsuarios_BulkInsert()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();
            var usuarios = new List<Usuario>();

            for (int i = 1; i <= 15; i++)
            {
                usuarios.Add(new Usuario
                {
                    Nombre = $"Usuario{i}",
                    Email = $"user{i}@example.com",
                    Password = $"pass{i}",
                    Rol = i % 5 == 0 ? "Admin" : "User"
                });
            }

            // Act
            context.Usuarios.AddRange(usuarios);
            context.SaveChanges();

            var count = context.Usuarios.Count();

            // Assert
            Assert.AreEqual(15, count);
        }

        [TestMethod]
        [TestCategory("Performance")]
        public void FilterHabitacionesByTipo_Performance()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();

            context.Habitaciones.AddRange(
                new Habitacion { Numero = "101", Tipo = "Suite", Precio = 150 },
                new Habitacion { Numero = "102", Tipo = "Doble", Precio = 100 },
                new Habitacion { Numero = "103", Tipo = "Suite", Precio = 150 },
                new Habitacion { Numero = "104", Tipo = "Simple", Precio = 75 }
            );
            context.SaveChanges();

            // Act
            var suites = context.Habitaciones.Where(h => h.Tipo == "Suite").ToList();

            // Assert
            Assert.AreEqual(2, suites.Count);
        }

        [TestMethod]
        [TestCategory("Performance")]
        public void AggregateHabitacionesByPrice()
        {
            // Arrange
            using var context = TestDatabaseHelper.CreateTestContext();

            context.Habitaciones.AddRange(
                new Habitacion { Numero = "101", Precio = 100 },
                new Habitacion { Numero = "102", Precio = 150 },
                new Habitacion { Numero = "103", Precio = 200 }
            );
            context.SaveChanges();

            // Act
            var totalPrice = context.Habitaciones.Sum(h => h.Precio);
            var averagePrice = context.Habitaciones.Average(h => h.Precio);

            // Assert
            Assert.AreEqual(450m, totalPrice);
            Assert.IsTrue(averagePrice > 0);
        }
    }
}
