using HotelCc.Controllers;
using HotelCc.Data;
using HotelCc.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestProject1
{
    [TestClass]
    public class ReservaTests
    {
        // =========================
        // DB EN MEMORIA
        // =========================
        private AppDbContext GetDatabaseContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new AppDbContext(options);
        }

        // =========================
        // RESERVA VÁLIDA
        // =========================
        [TestMethod]
        public async Task ReservaValida()
        {
            // ARRANGE
            using var context = GetDatabaseContext("ReservaValidaDB");

            context.Habitaciones.Add(new Habitacion
            {
                Id = 1,
                Numero = "101",
                Tipo = "Simple",
                Precio = 100
            });

            await context.SaveChangesAsync();

            var controller = new ReservasController(context);

            var reserva = new Reserva
            {
                HabitacionId = 1,
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(2),
                FechaReserva = DateTime.Now,
                Total = 200
            };

            // ACT
            context.Reservas.Add(reserva);

            await context.SaveChangesAsync();

            // ASSERT
            Assert.AreEqual(1, context.Reservas.Count());
        }

        // =========================
        // FECHA INVÁLIDA
        // =========================
        [TestMethod]
        public async Task FechaSalidaMenorEntrada()
        {
            // ARRANGE
            using var context = GetDatabaseContext("FechaInvalidaDB");

            var reserva = new Reserva
            {
                HabitacionId = 1,
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(-1)
            };

            // ASSERT
            Assert.IsTrue(
                reserva.FechaSalida < reserva.FechaEntrada
            );
        }

        // =========================
        // HABITACIÓN OCUPADA
        // =========================
        [TestMethod]
        public async Task HabitacionOcupada()
        {
            // ARRANGE
            using var context = GetDatabaseContext("HabitacionOcupadaDB");

            context.Reservas.Add(new Reserva
            {
                HabitacionId = 1,
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(3)
            });

            await context.SaveChangesAsync();

            // NUEVA RESERVA
            var nuevaReserva = new Reserva
            {
                HabitacionId = 1,
                FechaEntrada = DateTime.Today.AddDays(1),
                FechaSalida = DateTime.Today.AddDays(2)
            };

            // ACT
            var ocupada = context.Reservas.Any(r =>
                r.HabitacionId == nuevaReserva.HabitacionId &&
                nuevaReserva.FechaEntrada <= r.FechaSalida &&
                nuevaReserva.FechaSalida >= r.FechaEntrada
            );

            // ASSERT
            Assert.IsTrue(ocupada);
        }

        // =========================
        // FECHAS NO CRUZADAS
        // =========================
        [TestMethod]
        public async Task HabitacionDisponible()
        {
            // ARRANGE
            using var context = GetDatabaseContext("DisponibleDB");

            context.Reservas.Add(new Reserva
            {
                HabitacionId = 1,
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(2)
            });

            await context.SaveChangesAsync();

            // NUEVA RESERVA
            var nuevaReserva = new Reserva
            {
                HabitacionId = 1,
                FechaEntrada = DateTime.Today.AddDays(5),
                FechaSalida = DateTime.Today.AddDays(7)
            };

            // ACT
            var ocupada = context.Reservas.Any(r =>
                r.HabitacionId == nuevaReserva.HabitacionId &&
                nuevaReserva.FechaEntrada <= r.FechaSalida &&
                nuevaReserva.FechaSalida >= r.FechaEntrada
            );

            // ASSERT
            Assert.IsFalse(ocupada);
        }
    }
}