using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelCc.ViewModels;
using System;

namespace HotelCc.Tests.ViewModels
{
    [TestClass]
    public class LoginViewModelTests
    {
        [TestMethod]
        [TestCategory("LoginViewModel")]
        public void LoginViewModel_Creation_WithValidData()
        {
            // Arrange & Act
            var model = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "password123"
            };

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual("test@example.com", model.Email);
            Assert.AreEqual("password123", model.Password);
        }

        [TestMethod]
        [TestCategory("LoginViewModel")]
        public void LoginViewModel_Email_CanBeNull()
        {
            // Arrange & Act
            var model = new LoginViewModel { Email = null };

            // Assert
            Assert.IsNull(model.Email);
        }

        [TestMethod]
        [TestCategory("LoginViewModel")]
        public void LoginViewModel_Password_CanBeNull()
        {
            // Arrange & Act
            var model = new LoginViewModel { Password = null };

            // Assert
            Assert.IsNull(model.Password);
        }

        [TestMethod]
        [TestCategory("LoginViewModel")]
        public void LoginViewModel_Properties_CanBeUpdated()
        {
            // Arrange
            var model = new LoginViewModel();

            // Act
            model.Email = "newemail@example.com";
            model.Password = "newpassword";

            // Assert
            Assert.AreEqual("newemail@example.com", model.Email);
            Assert.AreEqual("newpassword", model.Password);
        }

        [TestMethod]
        [TestCategory("LoginViewModel")]
        public void LoginViewModel_Multiple_Instances_AreIndependent()
        {
            // Arrange
            var model1 = new LoginViewModel { Email = "user1@example.com" };
            var model2 = new LoginViewModel { Email = "user2@example.com" };

            // Act & Assert
            Assert.AreNotEqual(model1.Email, model2.Email);
        }

        [TestMethod]
        [TestCategory("LoginViewModel")]
        public void LoginViewModel_EmptyEmail()
        {
            // Arrange & Act
            var model = new LoginViewModel { Email = "" };

            // Assert
            Assert.AreEqual("", model.Email);
        }

        [TestMethod]
        [TestCategory("LoginViewModel")]
        public void LoginViewModel_EmptyPassword()
        {
            // Arrange & Act
            var model = new LoginViewModel { Password = "" };

            // Assert
            Assert.AreEqual("", model.Password);
        }
    }
}
