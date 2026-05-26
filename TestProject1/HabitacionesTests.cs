using HotelCc.Controllers;
using HotelCc.Data;
using HotelCc.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestProject1
{
    [TestClass]
    public class HabitacionesTests
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
        // INDEX
        // =========================

        [TestMethod]
        public async Task IndexRetornaVista()
        {
            using var context = GetDatabaseContext("IndexDB");

            context.Habitaciones.Add(new Habitacion
            {
                Numero = "101",
                Tipo = "Simple",
                Precio = 100
            });

            await context.SaveChangesAsync();

            var controller = new HabitacionesController(context);

            var result = await controller.Index();

            Assert.IsInstanceOfType(
                result,
                typeof(ViewResult)
            );
        }

        // =========================
        // DETAILS OK
        // =========================

        [TestMethod]
        public async Task DetailsRetornaHabitacion()
        {
            using var context = GetDatabaseContext("DetailsDB");

            context.Habitaciones.Add(new Habitacion
            {
                Id = 1,
                Numero = "101",
                Tipo = "Simple",
                Precio = 100
            });

            await context.SaveChangesAsync();

            var controller = new HabitacionesController(context);

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
            using var context = GetDatabaseContext("DetailsNullDB");

            var controller = new HabitacionesController(context);

            var result = await controller.Details(null);

            Assert.IsInstanceOfType(
                result,
                typeof(NotFoundResult)
            );
        }

        // =========================
        // CREATE
        // =========================

        [TestMethod]
        public async Task CreateAgregaHabitacion()
        {
            using var context = GetDatabaseContext("CreateDB");

            var controller = new HabitacionesController(context);

            var habitacion = new Habitacion
            {
                Numero = "201",
                Tipo = "Doble",
                Precio = 200
            };

            var result = await controller.Create(habitacion);

            Assert.AreEqual(
                1,
                context.Habitaciones.Count()
            );

            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult)
            );
        }

        // =========================
        // DELETE
        // =========================

        [TestMethod]
        public async Task DeleteEliminaHabitacion()
        {
            using var context = GetDatabaseContext("DeleteDB");

            context.Habitaciones.Add(new Habitacion
            {
                Id = 1,
                Numero = "301",
                Tipo = "Suite",
                Precio = 350
            });

            await context.SaveChangesAsync();

            var controller = new HabitacionesController(context);

            var result = await controller.DeleteConfirmed(1);

            Assert.AreEqual(
                0,
                context.Habitaciones.Count()
            );

            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult)
            );
        }
    }
}