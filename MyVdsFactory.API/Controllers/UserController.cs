using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVdsFactory.Application.Users.Commands.CreateUser;
using MyVdsFactory.Application.Users.Commands.DeleteUser;
using MyVdsFactory.Application.Users.Commands.UpdateUser;
using MyVdsFactory.Application.Users.Queries.GetUser;
using MyVdsFactory.Application.Users.Queries.GetUserList;
using MyVdsFactory.Domain.Constants;

namespace MyVdsFactory.API.Controllers;

public class UserController : BaseController
{
    [HttpGet]
    [Route("list")]
    //[Authorize(Roles = $"{UserRoleConsts.Admin}")]
    public async Task<ActionResult<GetUserListVm>> GetAll(string? firstName, string? lastName)
    {
        return Ok(await Mediator.Send(new GetUserListQuery
        {
            FirstName = firstName,
            LastName = lastName
        }));
    }

    [HttpGet]
    [Route("get")]
    [Authorize(Roles = $"{UserRoleConsts.Admin}")]
    public async Task<ActionResult<GetUserVm>> Get(long id)
    {
        return Ok(await Mediator.Send(new GetUserQuery
        {
            Id = id
        }));
    }
    
    [HttpPost]
    [Route("add")]
    [AllowAnonymous]
    //[Authorize(Roles = $"{UserRoleConsts.Admin}")]
    public async Task<IActionResult> Create(CreateUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost]
    [Route("update")]
    [Authorize(Roles = $"{UserRoleConsts.Admin}")]
    public async Task<IActionResult> Update(UpdateUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost]
    [Route("delete")]
    [Authorize(Roles = $"{UserRoleConsts.Admin}")]
    public async Task<IActionResult> Delete(DeleteUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}