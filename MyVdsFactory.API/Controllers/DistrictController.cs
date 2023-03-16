using Microsoft.AspNetCore.Mvc;
using MyVdsFactory.API.Models;
using MyVdsFactory.Application.Districts.Commands.AddDistrict;
using MyVdsFactory.Application.Districts.Commands.DeleteDistrict;
using MyVdsFactory.Application.Districts.Commands.UpdateDistrict;
using MyVdsFactory.Application.Districts.Queries.GetDistrict;
using MyVdsFactory.Application.Districts.Queries.GetDistrictList;

namespace MyVdsFactory.API.Controllers;

public class DistrictController : BaseController
{
    [HttpGet]
    [Route("get")]
    public async Task<ActionResult<GetDistrictVm>> Get(long id)
    {
        return Ok(await Mediator.Send(new GetDistrictQuery
        {
            Id = id
        }));
    }
    
    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<GetDistrictListVm>> GetAll([FromQuery] GetDistrictListRequestModel model)
    {
        return Ok(await Mediator.Send(new GetDistrictListQuery
        {
            CityId = model.CityId,
            CityName = model.CityName
        }));
    }
    
    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(AddDistrictCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> Update(UpdateDistrictCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(DeleteDistrictCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}