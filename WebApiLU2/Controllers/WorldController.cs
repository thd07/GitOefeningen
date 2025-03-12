using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiLU2.DTOs;
using WebApiLU2.Models;
using WebApiLU2.Repository;

namespace WebApiLU2.Controllers
{
    [Route("api/worlds")]
    [ApiController]
    [Authorize]
    public class WorldController : ControllerBase
    {
        private readonly WorldRepository _worldRepo;

        public WorldController(WorldRepository worldRepo)
        {
            _worldRepo = worldRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserWorlds()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var worlds = await _worldRepo.GetUserWorldsAsync(userId);
            return Ok(worlds);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorld([FromBody] Environment2D model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var world = new Environment2D { Id = Guid.NewGuid(), Name = model.Name, MaxHeight = model.MaxHeight, MaxLength = model.MaxLength, UserId = userId };

            await _worldRepo.CreateWorldAsync(world);
            return Ok(new { Message = "World created" });
        }
    }
}
