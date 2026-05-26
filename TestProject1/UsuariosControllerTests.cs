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
    public class UsuariosControllerTests
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
        // SESSION ADMIN
        // =========================

        private void ConfigurarAdminSession(
            UsuariosController controller)
        {
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

            controller.ControllerContext =
                new ControllerContext()
                {
                    HttpContext = httpContext
                };
        }

        // =========================
        // INDEX
        // =========================

        [TestMethod]
        public async Task IndexRetornaVista()
        {
            using var context = GetDatabaseContext("UsuariosIndexDB");

            context.Usuarios.Add(new Usuario
            {
                Nombre = "Ronex",
                Email = "ronex@test.com",
                Password = "123456",
                Rol = "Admin"
            });

            await context.SaveChangesAsync();

            var controller =
                new UsuariosController(context);

            ConfigurarAdminSession(controller);

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
        public async Task DetailsRetornaUsuario()
        {
            using var context = GetDatabaseContext("UsuariosDetailsDB");

            context.Usuarios.Add(new Usuario
            {
                Id = 1,
                Nombre = "Ronex",
                Email = "ronex@test.com",
                Password = "123456",
                Rol = "Admin"
            });

            await context.SaveChangesAsync();

            var controller =
                new UsuariosController(context);

            ConfigurarAdminSession(controller);

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
            using var context = GetDatabaseContext("UsuariosNullDB");

            var controller =
                new UsuariosController(context);

            ConfigurarAdminSession(controller);

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
        public async Task CreateAgregaUsuario()
        {
            using var context = GetDatabaseContext("UsuariosCreateDB");

            var controller =
                new UsuariosController(context);

            ConfigurarAdminSession(controller);

            var usuario = new Usuario
            {
                Nombre = "Nuevo",
                Email = "nuevo@test.com",
                Password = "123456",
                Rol = "User"
            };

            var result = await controller.Create(usuario);

            Assert.AreEqual(
                1,
                context.Usuarios.Count()
            );

            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult)
            );
        }

        // =========================
        // EDIT
        // =========================

        [TestMethod]
        public async Task EditActualizaUsuario()
        {
            using var context = GetDatabaseContext("UsuariosEditDB");

            // USUARIO ORIGINAL
            context.Usuarios.Add(new Usuario
            {
                Id = 1,
                Nombre = "Ronex",
                Email = "ronex@test.com",
                Password = "123456",
                Rol = "User"
            });

            await context.SaveChangesAsync();

            // LIMPIAR TRACKING
            context.ChangeTracker.Clear();

            var controller =
                new UsuariosController(context);

            ConfigurarAdminSession(controller);

            // USUARIO EDITADO
            var usuarioEditado = new Usuario
            {
                Id = 1,
                Nombre = "Ronex Editado",
                Email = "ronex@test.com",
                Password = "123456",
                Rol = "Admin"
            };

            // ACT
            var result =
                await controller.Edit(1, usuarioEditado);

            // ASSERT RESULT
            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult)
            );

            // VALIDAR DB
            var usuarioDb =
                await context.Usuarios.FindAsync(1);

            Assert.IsNotNull(usuarioDb);

            Assert.AreEqual(
                "Admin",
                usuarioDb.Rol
            );
        }

        // =========================
        // DELETE
        // =========================

        [TestMethod]
        public async Task DeleteEliminaUsuario()
        {
            using var context = GetDatabaseContext("UsuariosDeleteDB");

            context.Usuarios.Add(new Usuario
            {
                Id = 1,
                Nombre = "Eliminar",
                Email = "eliminar@test.com",
                Password = "123456",
                Rol = "User"
            });

            await context.SaveChangesAsync();

            var controller =
                new UsuariosController(context);

            ConfigurarAdminSession(controller);

            var result =
                await controller.DeleteConfirmed(1);

            Assert.AreEqual(
                0,
                context.Usuarios.Count()
            );

            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult)
            );
        }

        // =========================
        // SIN ADMIN
        // =========================

        [TestMethod]
        public async Task SinAdminRedireccionaLogin()
        {
            using var context = GetDatabaseContext("UsuariosNoAdminDB");

            var controller =
                new UsuariosController(context);

            // SESSION VACÍA
            var sessionMock = new Mock<ISession>();

            byte[] value = null;

            sessionMock
                .Setup(s => s.TryGetValue(
                    "Rol",
                    out value))
                .Returns(false);

            var httpContext = new DefaultHttpContext();

            httpContext.Session = sessionMock.Object;

            controller.ControllerContext =
                new ControllerContext()
                {
                    HttpContext = httpContext
                };

            var result = await controller.Index();

            Assert.IsInstanceOfType(
                result,
                typeof(RedirectToActionResult)
            );
        }
    }
}