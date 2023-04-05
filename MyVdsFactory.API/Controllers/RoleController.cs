using Microsoft.AspNetCore.Mvc;
using MyVdsFactory.Application.Roles.Commands.AddRoleCommand;
using MyVdsFactory.Application.Roles.Queries.GetRoleList;

namespace MyVdsFactory.API.Controllers;

public class RoleController : BaseController
{
    [HttpGet]
    [Route("list")]
    //[Authorize(Roles = $"{UserRoleConsts.Admin}")]
    public async Task<ActionResult<GetRoleListVm>> GetAll(string? roleName)
    {
        return Ok(await Mediator.Send(new GetRoleListQuery
        {
            RoleName = roleName
        }));
    }
    
    [HttpPost]
    [Route("add")]
    //[Authorize(Roles = $"{UserRoleConsts.Admin}")]
    public async Task<IActionResult> Create(AddRoleCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}