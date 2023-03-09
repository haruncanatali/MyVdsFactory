using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVdsFactory.Application.Auth.Dtos;
using MyVdsFactory.Application.Auth.Queries.HardPasswordChange;
using MyVdsFactory.Application.Auth.Queries.Login;
using MyVdsFactory.Application.Common.Models;

namespace MyVdsFactory.API.Controllers;

[AllowAnonymous]
public class AuthController : BaseController
{
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<Result<LoginDto>>> Login(LoginQuery loginModel)
    {
        Result<LoginDto> loginResponse = await Mediator.Send(loginModel);
        return Ok(loginResponse);
    }
    
    [HttpPost]
    [Route("password-change")]
    public async Task<IActionResult> HardPasswordChange(HardPasswordChangeQuery request)
    {
        return Ok(await Mediator.Send(request));
    }
}