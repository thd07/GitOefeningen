using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebApiLU2.Models;
using WebApiLU2.Repository;
using WebApiLU2.Services;

namespace WebApiLU2.Controllers;

[Route("/objects")]
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
        var UserId = _IAuthenticationServices.GetCurrentAuthenticatedUserId();
        var objects = await _IObject2DRepository.ReadAsyncId(WorldId, Guid.Parse(UserId));
        return Ok(objects);
    }
    [HttpPost(Name = "CreateObject")]

    public async Task<IActionResult> Create2dObject(Object2D model)
    {
        var userId = _IAuthenticationServices.GetCurrentAuthenticatedUserId();
        var objects = await _IObject2DRepository.InsertAsync(model, Guid.Parse(userId));
        return Ok();
    }
    [HttpPut(Name ="UpdateObjects")]
    public async Task<IActionResult> UpdateAsync(Object2D objectModel)
    {
        var userId = _IAuthenticationServices.GetCurrentAuthenticatedUserId();
        await _IObject2DRepository.UpdateAsync(objectModel, Guid.Parse(userId));
        return Ok();
    }
    [HttpDelete(Name ="DeleteAllObjects")]
    public async Task<IActionResult> DeleteAllAsync(Guid WorldId, Guid UserId)
    {
        var userId = _IAuthenticationServices.GetCurrentAuthenticatedUserId();
        await _IObject2DRepository.DeleteAllAsync(WorldId, Guid.Parse(userId));
        return Ok();
    }
}
