using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApiLU2.Controllers;
using WebApiLU2.Repository;
using WebApiLU2.Services;
using WebApiLU2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TestProject1
{
    [TestClass]
    public class EnvironmentControllerTests
    {
        private Mock<IAuthenticationServices> _mockAuthService;
        private Mock<IEnvironment2dRepository> _mockRepo;
        private EnvironmentController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthenticationServices>();
            _mockRepo = new Mock<IEnvironment2dRepository>();
            _controller = new EnvironmentController(_mockAuthService.Object, _mockRepo.Object);
        }

        [TestMethod]
        public async Task Get_ReturnsOk_WithEnvironmentList()
        {
            // Arrange
            var fakeUserId = Guid.NewGuid().ToString();
            var fakeWorlds = new List<Environment2D> { new Environment2D { Name = "Test World" } };

            _mockAuthService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(fakeUserId);
            _mockRepo.Setup(x => x.ReadWorldsAsync(Guid.Parse(fakeUserId))).ReturnsAsync(fakeWorlds);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(List<Environment2D>));
        }

        [TestMethod]
        public async Task Get_ReturnsBadRequest_WhenUserIdIsNull()
        {
            _mockAuthService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns((string)null);

            var result = await _controller.Get();

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CreateEnvironment_ReturnsOk_WithNewEnvironment()
        {
            var fakeUserId = Guid.NewGuid().ToString();
            var model = new Environment2D { Name = "New World", MaxHeight = 10, MaxLength = 10 };
            var expectedResult = new Environment2D { Id = Guid.NewGuid(), Name = model.Name };

            _mockAuthService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(fakeUserId);
            _mockRepo.Setup(x => x.CreateWorldAsync(model, Guid.Parse(fakeUserId))).ReturnsAsync(expectedResult);

            var result = await _controller.CreateEnvironment(model);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(expectedResult, okResult.Value);
        }

        [TestMethod]
        public async Task DeleteEnvironment_ReturnsOk()
        {
            var fakeUserId = Guid.NewGuid().ToString();
            var worldId = Guid.NewGuid();

            _mockAuthService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(fakeUserId);
            _mockRepo.Setup(x => x.DeleteEnvironmentAsync(Guid.Parse(fakeUserId), worldId)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteEnvironment(worldId);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}
