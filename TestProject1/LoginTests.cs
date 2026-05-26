using HotelCc.Controllers;
using HotelCc.Data;
using HotelCc.Models;
using HotelCc.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestProject1
{
    [TestClass]
    public class LoginTests
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

         //=========================
         //LOGIN CORRECTO
         //=========================
        [TestMethod]
        public async Task LoginCorrecto()
        {
            // ARRANGE
            using var context = GetDatabaseContext("LoginCorrectoDB");

            context.Usuarios.Add(new Usuario
            {
                Nombre = "Ronex",
                Email = "ronex@test.com",
                Password = "123456",
                Rol = "User"
            });

            await context.SaveChangesAsync();

            var controller = new AuthController(context);

            var model = new LoginViewModel
            {
                Email = "ronex@test.com",
                Password = "123456"
            };

            // ACT
            var result = controller.Login(model);

            // ASSERT
            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult)
            );
        }

        // =========================
        // LoginCorrecto con Moq
        // =========================

        //[TestMethod]
        //public void LoginCorrecto()
        //{
        //    // =========================
        //    // DB EN MEMORIA
        //    // =========================

        //    using var context = GetDatabaseContext("LoginCorrectoDB");

        //    context.Usuarios.Add(new Usuario
        //    {
        //        Nombre = "Ronex",
        //        Email = "ronex@test.com",
        //        Password = "123456",
        //        Rol = "User"
        //    });

        //    context.SaveChanges();

        //    // =========================
        //    // CONTROLLER
        //    // =========================

        //    var controller = new AuthController(context);

        //    // =========================
        //    // MOCK SESSION
        //    // =========================

        //    var sessionMock = new Mock<ISession>();

        //    var httpContext = new DefaultHttpContext();

        //    httpContext.Session = sessionMock.Object;

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = httpContext
        //    };

        //    // =========================
        //    // MODEL LOGIN
        //    // =========================

        //    var model = new LoginViewModel
        //    {
        //        Email = "ronex@test.com",
        //        Password = "123456"
        //    };

        //    // =========================
        //    // ACT
        //    // =========================

        //    var result = controller.Login(model);

        //    // =========================
        //    // ASSERT
        //    // =========================

        //    Assert.IsInstanceOfType(
        //        result,
        //        typeof(RedirectToActionResult)
        //    );
        //}


        // =========================
        // PASSWORD INCORRECTA
        // =========================
        [TestMethod]
        public async Task PasswordIncorrecta()
        {
            // ARRANGE
            using var context = GetDatabaseContext("PasswordIncorrectaDB");

            context.Usuarios.Add(new Usuario
            {
                Nombre = "Ronex",
                Email = "ronex@test.com",
                Password = "123456",
                Rol = "User"
            });

            await context.SaveChangesAsync();

            var controller = new AuthController(context);

            var model = new LoginViewModel
            {
                Email = "ronex@test.com",
                Password = "999999"
            };

            // ACT
            var result = controller.Login(model);

            // ASSERT
            Assert.IsInstanceOfType(
                result,
                typeof(ViewResult)
            );
        }

        // =========================
        // EMAIL NO EXISTE
        // =========================
        [TestMethod]
        public async Task EmailNoExiste()
        {
            // ARRANGE
            using var context = GetDatabaseContext("EmailNoExisteDB");

            var controller = new AuthController(context);

            var model = new LoginViewModel
            {
                Email = "fake@test.com",
                Password = "123456"
            };

            // ACT
            var result = controller.Login(model);

            // ASSERT
            Assert.IsInstanceOfType(
                result,
                typeof(ViewResult)
            );
        }

        // =========================
        // CAMPOS VACIOS
        // =========================
        [TestMethod]
        public async Task CamposVacios()
        {
            // ARRANGE
            using var context = GetDatabaseContext("CamposVaciosDB");

            var controller = new AuthController(context);

            var model = new LoginViewModel
            {
                Email = "",
                Password = ""
            };

            controller.ModelState.AddModelError(
                "Email",
                "Campo requerido"
            );

            // ACT
            var result = controller.Login(model);

            // ASSERT
            Assert.IsInstanceOfType(
                result,
                typeof(ViewResult)
            );
        }
    }
}