using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebApiLU2.Models;
using WebApiLU2.Repository;
using WebApiLU2.Services;

namespace WebApiLU2.Controllers;

[Route("objects")]
[ApiController]
[Authorize]
public class ObjectController : ControllerBase

{
    private readonly IObject2dRepository _IObject2DRepository;
    private readonly IAuthenticationServices _IAuthenticationServices;
    public ObjectController(IAuthenticationServices iAuthenticationServices, IObject2dRepository Object2dRepository)
    {
        _IObject2DRepository = Object2dRepository;
        _IAuthenticationServices = iAuthenticationServices;
    }

    [HttpGet("{WorldId}", Name = "getAllObjects")]
        public async Task<ActionResult<IEnumerable<Object2D>>> GetAllObjects(Guid WorldId)
        {
         var objects = await _IObject2DRepository.ReadAsyncId(WorldId);
        return Ok(objects);
        }

    [HttpPost(Name = "CreateObject")]

        public async Task<IActionResult> Create2dObject([FromBody] Object2D model)
            {
                var objects = await _IObject2DRepository.InsertAsync(model);
                return Ok(objects);
            }

    [HttpPut(Name ="UpdateObjects")]
        public async Task<IActionResult> UpdateAsync(Object2D objectModel)
        {
            await _IObject2DRepository.UpdateAsync(objectModel);
            bool trueOrFalse = true;
        return Ok(trueOrFalse);
        }

    [HttpDelete(Name ="DeleteAllObjects")]
    public async Task<IActionResult> DeleteAllAsync(Guid WorldId)
    {
        await _IObject2DRepository.DeleteAllAsync(WorldId);
        return Ok();
    }
    [HttpDelete("{WorldId}/{IdObject}", Name = "DeleteObject")]
    public async Task<IActionResult> DeleteOneObjectAsync(Guid WorldId, Guid ObjectId)
    {
        
        await _IObject2DRepository.DeleteObjectAsync(WorldId,ObjectId);
        return Ok();
    }
}

