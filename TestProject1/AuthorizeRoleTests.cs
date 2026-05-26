using HotelCc.Filters;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

using Moq;

namespace TestProject1
{
    [TestClass]
    public class AuthorizeRoleTests
    {
        // =========================
        // CREAR CONTEXTO
        // =========================

        private ActionExecutingContext GetContext(
            ISession session)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Session = session;

            var actionContext = new ActionContext(
                httpContext,
                new RouteData(),
                new ActionDescriptor()
            );

            return new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                controller: null
            );
        }

        // =========================
        // USUARIO NO LOGEADO
        // =========================

        [TestMethod]
        public void UsuarioNoLogeado_RedireccionaLogin()
        {
            var sessionMock = new Mock<ISession>();

            byte[] value = null;

            sessionMock
                .Setup(s => s.TryGetValue(
                    "Usuario",
                    out value))
                .Returns(false);

            var context = GetContext(sessionMock.Object);

            var filter = new AuthorizeRoleAttribute("Admin");

            // ACT
            filter.OnActionExecuting(context);

            // ASSERT
            Assert.IsInstanceOfType(
                context.Result,
                typeof(RedirectToActionResult)
            );
        }

        // =========================
        // ROL INCORRECTO
        // =========================

        [TestMethod]
        public void RolIncorrecto_RedireccionaLogin()
        {
            var sessionMock = new Mock<ISession>();

            byte[] userBytes =
                System.Text.Encoding.UTF8.GetBytes("Ronex");

            byte[] roleBytes =
                System.Text.Encoding.UTF8.GetBytes("User");

            sessionMock
                .Setup(s => s.TryGetValue(
                    "Usuario",
                    out userBytes))
                .Returns(true);

            sessionMock
                .Setup(s => s.TryGetValue(
                    "Rol",
                    out roleBytes))
                .Returns(true);

            var context = GetContext(sessionMock.Object);

            var filter = new AuthorizeRoleAttribute("Admin");

            // ACT
            filter.OnActionExecuting(context);

            // ASSERT
            Assert.IsInstanceOfType(
                context.Result,
                typeof(RedirectToActionResult)
            );
        }

        // =========================
        // ADMIN CORRECTO
        // =========================

        [TestMethod]
        public void AdminCorrecto_NoRedirecciona()
        {
            var sessionMock = new Mock<ISession>();

            byte[] userBytes =
                System.Text.Encoding.UTF8.GetBytes("Ronex");

            byte[] roleBytes =
                System.Text.Encoding.UTF8.GetBytes("Admin");

            sessionMock
                .Setup(s => s.TryGetValue(
                    "Usuario",
                    out userBytes))
                .Returns(true);

            sessionMock
                .Setup(s => s.TryGetValue(
                    "Rol",
                    out roleBytes))
                .Returns(true);

            var context = GetContext(sessionMock.Object);

            var filter = new AuthorizeRoleAttribute("Admin");

            // ACT
            filter.OnActionExecuting(context);

            // ASSERT
            Assert.IsNull(context.Result);
        }
    }
}