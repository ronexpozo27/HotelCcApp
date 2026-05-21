using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using HotelCc.Controllers;
using HotelCc.Models;
using HotelCc.ViewModels;
using HotelCc.Tests.Helpers;
using System;
using System.Threading.Tasks;

namespace HotelCc.Tests.Controllers
{
    [TestClass]
    public class AuthControllerTests
    {
        private AuthController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            var context = TestDatabaseHelper.CreateTestContext();
            _controller = new AuthController(context);
        }

        [TestMethod]
        [TestCategory("AuthController")]
        public void Login_Get_ReturnsViewResult()
        {
            // Act
            var result = _controller.Login();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [TestCategory("AuthController")]
        public void Register_Get_ReturnsViewResult()
        {
            // Act
            var result = _controller.Register();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [TestCategory("AuthController")]
        public void Login_Get_ReturnedViewIsNotNull()
        {
            // Act
            var result = _controller.Login() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("AuthController")]
        public void Register_Get_ReturnedViewIsNotNull()
        {
            // Act
            var result = _controller.Register() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("AuthController")]
        public void Login_Post_WithNonExistentUser_ReturnView()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "nonexistent@example.com",
                Password = "password123"
            };

            // Act
            var result = _controller.Login(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [TestCategory("AuthController")]
        public void Login_Post_WithValidUser_RedirectsToReservas()
        {
            // Arrange
            var context = TestDatabaseHelper.CreateTestContext();
            var usuario = new Usuario
            {
                Nombre = "TestUser",
                Email = "test@example.com",
                Password = "password123",
                Rol = "User"
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            var controller = new AuthController(context);
            HttpContextMockHelper.SetupHttpContextWithSession(controller);

            var model = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "password123"
            };

            // Act
            var result = controller.Login(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Reservas", redirectResult.ControllerName);
        }

        [TestMethod]
        [TestCategory("AuthController")]
        public void Login_Post_WithAdminUser_RedirectsToAdmin()
        {
            // Arrange
            var context = TestDatabaseHelper.CreateTestContext();
            var usuario = new Usuario
            {
                Nombre = "AdminUser",
                Email = "admin@example.com",
                Password = "admin123",
                Rol = "Admin"
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            var controller = new AuthController(context);
            HttpContextMockHelper.SetupHttpContextWithAdminSession(controller);

            var model = new LoginViewModel
            {
                Email = "admin@example.com",
                Password = "admin123"
            };

            // Act
            var result = controller.Login(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Dashboard", redirectResult.ActionName);
            Assert.AreEqual("Admin", redirectResult.ControllerName);
        }

        [TestMethod]
        [TestCategory("AuthController")]
        public void Login_Post_WithIncorrectPassword_ReturnView()
        {
            // Arrange
            var context = TestDatabaseHelper.CreateTestContext();
            var usuario = new Usuario
            {
                Nombre = "TestUser",
                Email = "test@example.com",
                Password = "correctPassword",
                Rol = "User"
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            var controller = new AuthController(context);
            var model = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "wrongPassword"
            };

            // Act
            var result = controller.Login(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
