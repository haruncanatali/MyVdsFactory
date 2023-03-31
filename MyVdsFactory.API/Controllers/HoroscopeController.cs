using Microsoft.AspNetCore.Mvc;
using MyVdsFactory.Application.HoroscopeCommentaries.Commands.AddRangeHoroscopeCommentaryWithHtml;
using MyVdsFactory.Application.Horoscopes.Commands.AddHoroscopeCommand;
using MyVdsFactory.Application.Horoscopes.Commands.DeleteHoroscopeCommand;
using MyVdsFactory.Application.Horoscopes.Commands.UpdateHoroscopeCommand;
using MyVdsFactory.Application.Horoscopes.Queries.GetHoroscope;
using MyVdsFactory.Application.Horoscopes.Queries.GetHoroscopeList;

namespace MyVdsFactory.API.Controllers;

public class HoroscopeController : BaseController
{
    [HttpGet]
    [Route("get")]
    public async Task<ActionResult<GetHoroscopeVm>> Get(long id)
    {
        return Ok(await Mediator.Send(new GetHoroscopeQuery
        {
            Id = id
        }));
    }
    
    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<GetHoroscopeListVm>> GetAll(string? horoscopeName)
    {
        return Ok(await Mediator.Send(new GetHoroscopeListQuery{HoroscopeName = horoscopeName}));
    }
    
    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(AddHoroscopeCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> Update(UpdateHoroscopeCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(DeleteHoroscopeCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}