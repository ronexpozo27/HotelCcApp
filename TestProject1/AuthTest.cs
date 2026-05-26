using HotelCc.Controllers;
using HotelCc.Data;
using HotelCc.Models;
using HotelCc.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestProject1
{
    [TestClass]
    public class AuthTests
    {
        // =========================
        // CREAR DB EN MEMORIA
        // =========================
        private AppDbContext GetDatabaseContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new AppDbContext(options);
        }

        // =========================
        // REGISTRO CORRECTO
        // =========================
        [TestMethod]
        public async Task RegistroUsuarioCorrecto()
        {
            // ARRANGE
            using var context = GetDatabaseContext("RegistroDB");

            var controller = new AuthController(context);

            var model = new RegisterViewModel
            {
                Nombre = "Ronex",
                Email = "ronex@test.com",
                Password = "123456"
            };

            // ACT
            var result = await controller.Register(model);

            // ASSERT
            Assert.AreEqual(1, context.Usuarios.Count());

            Assert.AreEqual(
                "ronex@test.com",
                context.Usuarios.First().Email
            );

            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult)
            );
        }

        // =========================
        // EMAIL DUPLICADO
        // =========================
        [TestMethod]
        public async Task NoDebeRegistrarEmailDuplicado()
        {
            // ARRANGE
            using var context = GetDatabaseContext("DuplicadoDB");

            context.Usuarios.Add(new Usuario
            {
                Nombre = "Admin",
                Email = "admin@test.com",
                Password = "123456",
                Rol = "User"
            });

            await context.SaveChangesAsync();

            var controller = new AuthController(context);

            var model = new RegisterViewModel
            {
                Nombre = "Ronex",
                Email = "admin@test.com",
                Password = "654321"
            };

            // ACT
            var result = await controller.Register(model);

            // ASSERT

            // Debe seguir existiendo solo 1 usuario
            Assert.AreEqual(1, context.Usuarios.Count());

            // Debe retornar la misma vista
            Assert.IsInstanceOfType(
                result,
                typeof(ViewResult)
            );
        }
    }
}