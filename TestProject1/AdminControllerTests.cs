using HotelCc.Controllers;
using HotelCc.Data;
using HotelCc.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestProject1
{
    [TestClass]
    public class AdminControllerTests
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
        // DASHBOARD
        // =========================

        [TestMethod]
        public async Task DashboardRetornaVista()
        {
            using var context = GetDatabaseContext("AdminDashboardDB");

            // =========================
            // USUARIO
            // =========================

            context.Usuarios.Add(new Usuario
            {
                Id = 1,
                Nombre = "Admin",
                Email = "admin@test.com",
                Password = "123456",
                Rol = "Admin"
            });

            // =========================
            // HABITACIONES
            // =========================

            context.Habitaciones.Add(new Habitacion
            {
                Id = 1,
                Numero = "101",
                Tipo = "Simple",
                Precio = 100
            });

            context.Habitaciones.Add(new Habitacion
            {
                Id = 2,
                Numero = "201",
                Tipo = "Doble",
                Precio = 200
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

            var controller = new AdminController(context);

            // =========================
            // ACT
            // =========================

            var result = await controller.Dashboard();

            // =========================
            // ASSERT
            // =========================

            Assert.IsInstanceOfType(
                result,
                typeof(ViewResult)
            );

            // =========================
            // VIEWBAGS
            // =========================

            Assert.AreEqual(
                2,
                controller.ViewBag.TotalHabitaciones
            );

            Assert.AreEqual(
                1,
                controller.ViewBag.TotalReservas
            );

            Assert.AreEqual(
                1,
                controller.ViewBag.TotalUsuarios
            );

            Assert.AreEqual(
                1,
                controller.ViewBag.HabitacionesDisponibles
            );

            Assert.AreEqual(
                200m,
                controller.ViewBag.Ingresos
            );
        }
    }
}