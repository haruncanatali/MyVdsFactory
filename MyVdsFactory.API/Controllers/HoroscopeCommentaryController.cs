using Microsoft.AspNetCore.Mvc;
using MyVdsFactory.Application.HoroscopeCommentaries.Commands.AddHoroscopeCommentary;
using MyVdsFactory.Application.HoroscopeCommentaries.Commands.AddRangeHoroscopeCommentaryWithHtml;
using MyVdsFactory.Application.HoroscopeCommentaries.Commands.DeleteHoroscopeCommentary;
using MyVdsFactory.Application.HoroscopeCommentaries.Commands.UpdateHoroscopeCommentary;
using MyVdsFactory.Application.HoroscopeCommentaries.Queries.GetHoroscopeCommentary;
using MyVdsFactory.Application.HoroscopeCommentaries.Queries.GetHoroscopeCommentaryList;

namespace MyVdsFactory.API.Controllers;

public class HoroscopeCommentaryController : BaseController
{
    [HttpGet]
    [Route("get")]
    public async Task<ActionResult<GetHoroscopeCommentaryVm>> Get(long id)
    {
        return Ok(await Mediator.Send(new GetHoroscopeCommentaryQuery
        {
            Id = id
        }));
    }
    
    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<GetHoroscopeCommentaryListVm>> GetAll(long id)
    {
        return Ok(await Mediator.Send(new GetHoroscopeCommentaryListQuery
        {
            HoroscopeId = id
        }));
    }
    
    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(AddHoroscopeCommentaryCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPost]
    [Route("add-range-with-html")]
    public async Task<IActionResult> Add()
    {
        return Ok(await Mediator.Send(new AddRangeHoroscopeCommentaryWithHtmlCommand()));
    }
    
    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> Update(UpdateHoroscopeCommentaryCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(DeleteHoroscopeCommentaryCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}