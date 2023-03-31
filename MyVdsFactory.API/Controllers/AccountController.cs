using Microsoft.AspNetCore.Mvc;
using MyVdsFactory.Application.Accounts.Commands.AddAccountCommand;
using MyVdsFactory.Application.Accounts.Commands.DeleteAccountCommand;
using MyVdsFactory.Application.Accounts.Commands.UpdateAccountCommand;
using MyVdsFactory.Application.Accounts.Queries.GetAccount;
using MyVdsFactory.Application.Accounts.Queries.GetAccountList;

namespace MyVdsFactory.API.Controllers;

public class AccountController : BaseController
{
    [HttpGet]
    [Route("get")]
    public async Task<ActionResult<GetAccountVm>> Get(long id)
    {
        return Ok(await Mediator.Send(new GetAccountQuery
        {
            Id = id
        }));
    }
    
    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<GetAccountListVm>> GetAll(string? platform)
    {
        return Ok(await Mediator.Send(new GetAccountListQuery
        {
            Platform = platform
        }));
    }
    
    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(AddAccountCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> Update(UpdateAccountCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(DeleteAccountCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}