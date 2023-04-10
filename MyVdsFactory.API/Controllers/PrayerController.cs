using Microsoft.AspNetCore.Mvc;
using MyVdsFactory.Application.Prayers.Commands.AddPrayer;
using MyVdsFactory.Application.Prayers.Commands.AddPrayerWithHtml;
using MyVdsFactory.Application.Prayers.Commands.DeletePrayer;
using MyVdsFactory.Application.Prayers.Commands.UpdatePrayer;
using MyVdsFactory.Application.Prayers.Queries.GetPrayer;
using MyVdsFactory.Application.Prayers.Queries.GetPrayerList;

namespace MyVdsFactory.API.Controllers;

public class PrayerController : BaseController
{
    [HttpGet]
    [Route("get")]
    public async Task<ActionResult<GetPrayerVm>> Get([FromQuery] long cityId,DateTime date)
    {
        return Ok(await Mediator.Send(new GetPrayerQuery
        {
            CityId = cityId,
            Date = date
        }));
    }
    
    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<GetPrayerListVm>> GetAll([FromQuery] long cityId,int month,int year)
    {
        return Ok(await Mediator.Send(new GetPrayerListQuery
        {
            CityId = cityId,
            Year = year,
            Month = month
        }));
    }
    
    [HttpPost]
    [Route("add-with-html")]
    public async Task<IActionResult> Add()
    {
        return Ok(await Mediator.Send(new AddPrayerWithHtmlCommand()));
    }
    
    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(AddPrayerCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> Update(UpdatePrayerCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(DeletePrayerCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}