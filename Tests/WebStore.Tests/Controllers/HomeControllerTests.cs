﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController _Controller;

        [TestInitialize]
        public void Initialize()
        {
            var logger_mock = new Mock<ILogger<HomeController>>();
            _Controller = new HomeController(logger_mock.Object);
        }

        [TestMethod]
        public void Index_Returns_View()
        {
            var result = _Controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void Blog_Returns_View()
        {
            var result = _Controller.Blog();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void BlogSingle_Returns_View()
        {
            var result = _Controller.BlogSingle();

            Assert.IsType<ViewResult>(result);
        } 

        [TestMethod]
        public void ContactUs_Returns_View()
        {
            var result = _Controller.ContactUs();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod, ExpectedException(typeof(ApplicationException))]
        public void ThrowError_Throw_ApplicationException()
        {
            var result = _Controller.ThrowError(null);
        }

        [TestMethod]
        public void ThrowError_Throw_ApplicationException2()
        {
            const string expected_exception_message = "Hello World!";

            var exception = Assert.Throws<ApplicationException>(() => _Controller.ThrowError(expected_exception_message));

            Assert.Equal(expected_exception_message, exception.Message);
        }

        [TestMethod]
        public void Error404_Returns_View()
        {
            var result = _Controller.Error404();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void ErrorStatus_404_Redirect_to_Error404()
        {
            const string status_code = "404";
            var result = _Controller.ErrorStatus(status_code);

            var redirect_to_action = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirect_to_action.ControllerName);
            Assert.Equal(nameof(HomeController.Error404), redirect_to_action.ActionName);
        }
    }
}
