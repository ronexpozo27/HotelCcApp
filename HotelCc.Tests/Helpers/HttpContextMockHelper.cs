using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelCc.Tests.Helpers
{
    public static class HttpContextMockHelper
    {
        public static void SetupHttpContextWithSession(Controller controller)
        {
            var sessionMock = new MockSession();
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(ctx => ctx.Session).Returns(sessionMock);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContextMock.Object
            };
        }

        public static void SetupHttpContextWithAdminSession(Controller controller)
        {
            var sessionMock = new MockSession { Role = "Admin" };
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(ctx => ctx.Session).Returns(sessionMock);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContextMock.Object
            };
        }
    }

    public class MockSession : ISession
    {
        private Dictionary<string, byte[]> _sessionStorage = new Dictionary<string, byte[]>();
        public string Role { get; set; } = "User";

        public IEnumerable<string> Keys => _sessionStorage.Keys;

        public string Id { get; } = Guid.NewGuid().ToString();

        public bool IsAvailable { get; } = true;

        public void Clear()
        {
            _sessionStorage.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            _sessionStorage.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _sessionStorage[key] = value;
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            return _sessionStorage.TryGetValue(key, out value!);
        }

        // Métodos adicionales para compatibilidad con SessionExtensions
        public void SetString(string key, string value)
        {
            Set(key, System.Text.Encoding.UTF8.GetBytes(value));
        }

        public string? GetString(string key)
        {
            if (TryGetValue(key, out var value))
            {
                return System.Text.Encoding.UTF8.GetString(value);
            }

            // Simular valores de sesión específicos para pruebas
            if (key == "Rol")
                return Role;
            if (key == "Usuario")
                return "TestUser";

            return null;
        }

        public void SetInt32(string key, int value)
        {
            Set(key, System.BitConverter.GetBytes(value));
        }

        public int? GetInt32(string key)
        {
            if (TryGetValue(key, out var value))
            {
                return System.BitConverter.ToInt32(value, 0);
            }
            return null;
        }
    }
}

