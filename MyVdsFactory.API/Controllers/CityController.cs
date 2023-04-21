using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVdsFactory.API.Models;
using MyVdsFactory.API.Services;
using MyVdsFactory.Application.Cities.Commands.AddCityCommand;
using MyVdsFactory.Application.Cities.Commands.AddCityWithExcel;
using MyVdsFactory.Application.Cities.Commands.DeleteCityCommand;
using MyVdsFactory.Application.Cities.Commands.UpdateCityCommand;
using MyVdsFactory.Application.Cities.Queries.GetCity;
using MyVdsFactory.Application.Cities.Queries.GetCityList;
using MyVdsFactory.Application.Cities.Queries.GetCityListWithoutDistricts;

namespace MyVdsFactory.API.Controllers;

public class CityController : BaseController
{
    private readonly IFileServices _fileServices;

    public CityController(IFileServices fileServices)
    {
        _fileServices = fileServices;
    }

    [HttpPost]
    [Route("addExcel")]
    [AllowAnonymous]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> AddExcel(IFormFile excelFile)
    {
        var copyResult = await _fileServices.SaveFileAsync(excelFile, ModalPaths.ProvinceDistrict);

        if (copyResult)
        {
            return Ok(await Mediator.Send(new AddCityWithExcelCommand
            {
                DataExcelFile = excelFile
            }));
        }

        return BadRequest("Dosya kaydedilemedi.");
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<ActionResult<GetCityVm>> Get(long id)
    {
        return Ok(await Mediator.Send(new GetCityQuery
        {
            Id = id
        }));
    }
    
    [HttpGet]
    [Route("getWithoutDistricts")]
    public async Task<ActionResult<GetCityVm>> GetWithoutDistricts(string? name)
    {
        return Ok(await Mediator.Send(new GetCityListWithoutDistrictsQuery
        {
            Name = name
        }));
    }
    
    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<GetCityListVm>> GetAll([FromQuery] string? cityName)
    {
        return Ok(await Mediator.Send(new GetCityListQuery
        {
            Name = cityName
        }));
    }
    
    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(AddCityCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> Update(UpdateCityCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(DeleteCityCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}