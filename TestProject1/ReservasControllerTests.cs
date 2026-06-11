using HotelCc.Controllers;
using HotelCc.Data;
using HotelCc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestProject1
{
    [TestClass]
    public class ReservasControllerTests
    {
        // =========================
        // DB MEMORIA
        // =========================

        private AppDbContext GetDatabaseContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new AppDbContext(options);
        }

        // =========================
        // INDEX RETORNA VIEW
        // =========================

        [TestMethod]
        public async Task IndexRetornaVista()
        {
            using var context = GetDatabaseContext("ReservasIndexDB");

            // =========================
            // USUARIO
            // =========================

            context.Usuarios.Add(new Usuario
            {
                Id = 1,
                Nombre = "Ronex",
                Email = "ronex@test.com",
                Password = "123456",
                Rol = "Admin"
            });

            // =========================
            // HABITACION
            // =========================

            context.Habitaciones.Add(new Habitacion
            {
                Id = 1,
                Numero = "101",
                Tipo = "Simple",
                Precio = 100
            });

            // =========================
            // RESERVA
            // =========================

            context.Reservas.Add(new Reserva
            {
                Id = 1,
                UsuarioId = 1,
                HabitacionId = 1,
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(2),
                FechaReserva = DateTime.Now,
                Total = 200
            });

            await context.SaveChangesAsync();

            var controller = new ReservasController(context);

            // =========================
            // MOCK SESSION
            // =========================

            var sessionMock = new Mock<ISession>();

            byte[] roleBytes =
                System.Text.Encoding.UTF8.GetBytes("Admin");

            sessionMock
                .Setup(s => s.TryGetValue(
                    "Rol",
                    out roleBytes))
                .Returns(true);

            var httpContext = new DefaultHttpContext();

            httpContext.Session = sessionMock.Object;

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // =========================
            // ACT
            // =========================

            var result = await controller.Index();

            // =========================
            // ASSERT
            // =========================

            Assert.IsInstanceOfType(
                result,
                typeof(ViewResult)
            );
        }

        // =========================
        // DETAILS OK
        // =========================

        [TestMethod]
        public async Task DetailsRetornaReserva()
        {
            using var context = GetDatabaseContext("ReservaDetailsDB");

            // USUARIO
            context.Usuarios.Add(new Usuario
            {
                Id = 1,
                Nombre = "Ronex",
                Email = "ronex@test.com",
                Password = "123456",
                Rol = "User"
            });

            // HABITACIÓN
            context.Habitaciones.Add(new Habitacion
            {
                Id = 1,
                Numero = "101",
                Tipo = "Simple",
                Precio = 100
            });

            // RESERVA
            context.Reservas.Add(new Reserva
            {
                Id = 1,
                UsuarioId = 1,
                HabitacionId = 1,
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(2),
                FechaReserva = DateTime.Now,
                Total = 200
            });

            await context.SaveChangesAsync();

            var controller = new ReservasController(context);

            var result = await controller.Details(1);

            Assert.IsInstanceOfType(
                result,
                typeof(ViewResult)
            );
        }

        // =========================
        // DETAILS NULL
        // =========================

        [TestMethod]
        public async Task DetailsNullRetornaNotFound()
        {
            using var context = GetDatabaseContext("ReservaNullDB");

            var controller = new ReservasController(context);

            var result = await controller.Details(null);

            Assert.IsInstanceOfType(
                result,
                typeof(NotFoundResult)
            );
        }

        // =========================
        // CREATE RESERVA
        // =========================

        [TestMethod]
        public async Task CreateAgregaReserva()
        {
            using var context = GetDatabaseContext("ReservaCreateDB");

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

            var result = await controller.Create(reserva);

            Assert.AreEqual(
                1,
                context.Reservas.Count()
            );

            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult)
            );
        }

        // =========================
        // DELETE RESERVA
        // =========================

        [TestMethod]
        public async Task DeleteEliminaReserva()
        {
            using var context = GetDatabaseContext("ReservaDeleteDB");

            context.Reservas.Add(new Reserva
            {
                Id = 1,
                HabitacionId = 1,
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(2),
                FechaReserva = DateTime.Now,
                Total = 200
            });

            await context.SaveChangesAsync();

            var controller = new ReservasController(context);

            var result = await controller.DeleteConfirmed(1);

            Assert.AreEqual(
                0,
                context.Reservas.Count()
            );

            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult)
            );
        }

        // =========================
        // VALIDAR FECHAS
        // =========================

        [TestMethod]
        public void FechaSalidaMenorEntrada()
        {
            var reserva = new Reserva
            {
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(-1)
            };

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
            using var context = GetDatabaseContext("ReservaOcupadaDB");

            context.Reservas.Add(new Reserva
            {
                HabitacionId = 1,
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(3)
            });

            await context.SaveChangesAsync();

            var nuevaReserva = new Reserva
            {
                HabitacionId = 1,
                FechaEntrada = DateTime.Today.AddDays(1),
                FechaSalida = DateTime.Today.AddDays(2)
            };

            var ocupada = context.Reservas.Any(r =>
                r.HabitacionId == nuevaReserva.HabitacionId &&
                nuevaReserva.FechaEntrada <= r.FechaSalida &&
                nuevaReserva.FechaSalida >= r.FechaEntrada
            );

            Assert.IsTrue(ocupada);
        }
    }
}