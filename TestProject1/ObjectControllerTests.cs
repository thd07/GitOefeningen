using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApiLU2.Controllers;
using WebApiLU2.Repository;
using WebApiLU2.Services;
using WebApiLU2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class ObjectControllerTests
    {
        private Mock<IAuthenticationServices> _mockAuthService;
        private Mock<IObject2dRepository> _mockRepo;
        private ObjectController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthenticationServices>();
            _mockRepo = new Mock<IObject2dRepository>();
            _controller = new ObjectController(_mockAuthService.Object, _mockRepo.Object);
        }

        [TestMethod]
        public async Task GetAllObjects_ReturnsOk_WithObjectList()
        {
            // Arrange
            var fakeUserId = Guid.NewGuid().ToString();
            var fakeWorldId = Guid.NewGuid();
            var fakeObjects = new List<Object2D> { new Object2D { PrefabId = 1 } };

            _mockAuthService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(fakeUserId);
            _mockRepo.Setup(x => x.ReadAsyncId(fakeWorldId, Guid.Parse(fakeUserId))).ReturnsAsync(fakeObjects);

            // Act
            var result = await _controller.GetAllObjects(fakeWorldId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(List<Object2D>));
        }

        [TestMethod]
        public async Task GetAllObjects_ReturnsBadRequest_WhenUserIdIsNull()
        {
            _mockAuthService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns((string)null);
            var fakeWorldId = Guid.NewGuid();

            var result = await _controller.GetAllObjects(fakeWorldId);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Create2dObject_ReturnsOk_WithCreatedObject()
        {
            // Arrange
            var fakeUserId = Guid.NewGuid().ToString();
            var model = new Object2D { PrefabId = 1, PosX = 5, PosY = 10 };
            var createdObject = new Object2D { IdObject = Guid.NewGuid(), PrefabId = model.PrefabId };

            _mockAuthService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(fakeUserId);
            _mockRepo.Setup(x => x.InsertAsync(model, Guid.Parse(fakeUserId))).ReturnsAsync(createdObject);

            // Act
            var result = await _controller.Create2dObject(model);
            var okResult = result as OkResult;

            // Assert
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public async Task UpdateAsync_ReturnsOk()
        {
            // Arrange
            var fakeUserId = Guid.NewGuid().ToString();
            var model = new Object2D { IdObject = Guid.NewGuid(), PrefabId = 1, PosX = 5, PosY = 10 };

            _mockAuthService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(fakeUserId);
            _mockRepo.Setup(x => x.UpdateAsync(model, Guid.Parse(fakeUserId))).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateAsync(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteAllAsync_ReturnsOk()
        {
            // Arrange
            var fakeUserId = Guid.NewGuid().ToString();
            var worldId = Guid.NewGuid();

            _mockAuthService.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(fakeUserId);
            _mockRepo.Setup(x => x.DeleteAllAsync(worldId, Guid.Parse(fakeUserId))).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteAllAsync(worldId, Guid.Parse(fakeUserId));

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}

