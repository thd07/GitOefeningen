using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApiLU2.Controllers;
using WebApiLU2.Repository;
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
        private Mock<IObject2dRepository> _mockRepo;
        private ObjectController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IObject2dRepository>();
            _controller = new ObjectController(null, _mockRepo.Object); // Auth service no longer needed
        }

        [TestMethod]
        public async Task GetAllObjects_ReturnsOk_WithObjectList()
        {
            // Arrange
            var fakeWorldId = Guid.NewGuid();
            var fakeObjects = new List<Object2D> { new Object2D { PrefabId = "Prefab_1" } };

            _mockRepo.Setup(x => x.ReadAsyncId(fakeWorldId)).ReturnsAsync(fakeObjects);

            // Act
            var result = await _controller.GetAllObjects(fakeWorldId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(IEnumerable<Object2D>));
        }

        [TestMethod]
        public async Task Create2dObject_ReturnsOk()
        {
            // Arrange
            var model = new Object2D { PrefabId = "Prefab_1", PosX = 5, PosY = 10 };
            _mockRepo.Setup(x => x.InsertAsync(model)).ReturnsAsync(model);

            // Act
            var result = await _controller.Create2dObject(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task UpdateAsync_ReturnsOk()
        {
            // Arrange
            var model = new Object2D { IdObject = Guid.NewGuid(), PrefabId = "Prefab_1", PosX = 5, PosY = 10 };
            _mockRepo.Setup(x => x.UpdateAsync(model)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateAsync(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteAllAsync_ReturnsOk()
        {
            // Arrange
            var worldId = Guid.NewGuid();
            _mockRepo.Setup(x => x.DeleteAllAsync(worldId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteAllAsync(worldId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteOneObjectAsync_ReturnsOk()
        {
            // Arrange
            var worldId = Guid.NewGuid();
            var objectId = Guid.NewGuid();
            _mockRepo.Setup(x => x.DeleteObjectAsync(worldId, objectId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteOneObjectAsync(worldId, objectId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}
