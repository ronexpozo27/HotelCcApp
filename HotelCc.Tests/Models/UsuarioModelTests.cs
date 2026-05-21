using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelCc.Models;
using System;

namespace HotelCc.Tests.Models
{
    [TestClass]
    public class UsuarioModelTests
    {
        [TestMethod]
        [TestCategory("UsuarioModel")]
        public void Usuario_Creation_WithValidData_ShouldSucceed()
        {
            // Arrange & Act
            var usuario = new Usuario
            {
                Id = 1,
                Nombre = "Juan Pérez",
                Email = "juan@example.com",
                Password = "password123",
                Rol = "User"
            };

            // Assert
            Assert.IsNotNull(usuario);
            Assert.AreEqual(1, usuario.Id);
            Assert.AreEqual("Juan Pérez", usuario.Nombre);
            Assert.AreEqual("juan@example.com", usuario.Email);
            Assert.AreEqual("password123", usuario.Password);
            Assert.AreEqual("User", usuario.Rol);
        }

        [TestMethod]
        [TestCategory("UsuarioModel")]
        public void Usuario_Nombre_CanBeNull()
        {
            // Arrange & Act
            var usuario = new Usuario { Nombre = null };

            // Assert
            Assert.IsNull(usuario.Nombre);
        }

        [TestMethod]
        [TestCategory("UsuarioModel")]
        public void Usuario_Email_CanBeNull()
        {
            // Arrange & Act
            var usuario = new Usuario { Email = null };

            // Assert
            Assert.IsNull(usuario.Email);
        }

        [TestMethod]
        [TestCategory("UsuarioModel")]
        public void Usuario_Password_CanBeNull()
        {
            // Arrange & Act
            var usuario = new Usuario { Password = null };

            // Assert
            Assert.IsNull(usuario.Password);
        }

        [TestMethod]
        [TestCategory("UsuarioModel")]
        public void Usuario_Rol_CanBeNull()
        {
            // Arrange & Act
            var usuario = new Usuario { Rol = null };

            // Assert
            Assert.IsNull(usuario.Rol);
        }

        [TestMethod]
        [TestCategory("UsuarioModel")]
        public void Usuario_AdminRole_IsValid()
        {
            // Arrange & Act
            var usuario = new Usuario { Rol = "Admin" };

            // Assert
            Assert.AreEqual("Admin", usuario.Rol);
        }

        [TestMethod]
        [TestCategory("UsuarioModel")]
        public void Usuario_UserRole_IsValid()
        {
            // Arrange & Act
            var usuario = new Usuario { Rol = "User" };

            // Assert
            Assert.AreEqual("User", usuario.Rol);
        }

        [TestMethod]
        [TestCategory("UsuarioModel")]
        public void Usuario_Email_WithStandardFormat()
        {
            // Arrange & Act
            var usuario = new Usuario { Email = "test@domain.com" };

            // Assert
            Assert.AreEqual("test@domain.com", usuario.Email);
            Assert.IsTrue(usuario.Email.Contains("@"));
        }

        [TestMethod]
        [TestCategory("UsuarioModel")]
        public void Usuario_Multiple_Instances_AreIndependent()
        {
            // Arrange
            var usuario1 = new Usuario { Id = 1, Email = "user1@example.com" };
            var usuario2 = new Usuario { Id = 2, Email = "user2@example.com" };

            // Act & Assert
            Assert.AreNotEqual(usuario1.Id, usuario2.Id);
            Assert.AreNotEqual(usuario1.Email, usuario2.Email);
        }

        [TestMethod]
        [TestCategory("UsuarioModel")]
        public void Usuario_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var usuario = new Usuario();

            // Act
            usuario.Id = 10;
            usuario.Nombre = "Carlos López";
            usuario.Email = "carlos@example.com";
            usuario.Password = "pass123";
            usuario.Rol = "Admin";

            // Assert
            Assert.AreEqual(10, usuario.Id);
            Assert.AreEqual("Carlos López", usuario.Nombre);
            Assert.AreEqual("carlos@example.com", usuario.Email);
            Assert.AreEqual("pass123", usuario.Password);
            Assert.AreEqual("Admin", usuario.Rol);
        }
    }
}
