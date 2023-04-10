using Microsoft.AspNetCore.Mvc;
using MyVdsFactory.Application.Tarots.Commands.AddTarot;
using MyVdsFactory.Application.Tarots.Commands.AddTarotWithHtml;
using MyVdsFactory.Application.Tarots.Commands.DeleteTarot;
using MyVdsFactory.Application.Tarots.Commands.UpdateTarot;
using MyVdsFactory.Application.Tarots.Queries.GetRandomTarotList;
using MyVdsFactory.Application.Tarots.Queries.GetTarot;
using MyVdsFactory.Application.Tarots.Queries.GetTarotList;

namespace MyVdsFactory.API.Controllers;

public class TarotController : BaseController
{
    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<GetTarotListVm>> GetAll([FromQuery] string? name)
    {
        return Ok(await Mediator.Send(new GetTarotListQuery
        {
            Name = name
        }));
    }
    
    [HttpGet]
    [Route("list-random")]
    public async Task<ActionResult<GetTarotListVm>> GetRandomList([FromQuery] int? amount)
    {
        return Ok(await Mediator.Send(new GetRandomTarotListQuery
        {
            Amount = amount
        }));
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<ActionResult<GetTarotVm>> Get(long id)
    {
        return Ok(await Mediator.Send(new GetTarotQuery
        {
            Id = id
        }));
    }
    
    [HttpPost]
    [Route("add-with-html")]
    public async Task<IActionResult> AddWithHtml()
    {
        return Ok(await Mediator.Send(new AddTarotWithHtmlCommand()));
    }
    
    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(AddTarotCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> Update(UpdateTarotCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(DeleteTarotCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}