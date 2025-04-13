using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

using WebApiLU2.Models;
using System.Security.Claims;
using WebApiLU2.Services;
using Microsoft.Data.SqlClient;
using WebApiLU2.Repository;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApiLU2.Controllers;

[Route("/environment")]
[ApiController]
[Authorize]
public class EnvironmentController : ControllerBase
{

    private readonly IAuthenticationServices _IAuthenticationServices;
    private readonly IEnvironment2dRepository _environment2dRepository;

    public EnvironmentController(IAuthenticationServices iAuthenticationServices, IEnvironment2dRepository environment2DRepository)
    {

        _IAuthenticationServices = iAuthenticationServices;
        _environment2dRepository = environment2DRepository;
    }
  

    [HttpGet(Name = "GetLevels")]
    public async Task<ActionResult<Environment2D>> Get()
    {
        var UserId = _IAuthenticationServices.GetCurrentAuthenticatedUserId();
        if(UserId == null)
        {
            return BadRequest("UserId is null");
        }
        var environment = await _environment2dRepository.ReadWorldsAsync(Guid.Parse(UserId));

        return Ok(environment);
    }

    [HttpPost("{UserId}",Name = "MakeLevel")]
    public async Task<IActionResult> CreateEnvironment(Environment2D model)
    {
        var UserId = _IAuthenticationServices.GetCurrentAuthenticatedUserId();
        if (UserId == null)
        {
            return BadRequest("UserId is null");
        }
        var NewWorld = await _environment2dRepository.CreateWorldAsync(model,Guid.Parse(UserId));
        return Ok(NewWorld);
    }


    [HttpDelete("{WorldId}", Name ="DeleteLevel")]
    public async Task<IActionResult> DeleteEnvironment(Guid WorldId)
    {
        var UserId = _IAuthenticationServices.GetCurrentAuthenticatedUserId();
        if (UserId == null)
        {
            return BadRequest("UserId is null");
        }
        await _environment2dRepository.DeleteEnvironmentAsync(Guid.Parse(UserId),WorldId);
        return Ok();

    }
}


