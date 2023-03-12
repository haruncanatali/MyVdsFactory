﻿using Microsoft.AspNetCore.Mvc;
using MyVdsFactory.API.Models;
using MyVdsFactory.Application.Earthquakes.Commands.AddEarthquake;
using MyVdsFactory.Application.Earthquakes.Commands.AddEarthquakeWithExcel;
using MyVdsFactory.Application.Earthquakes.Commands.DeleteEarthquake;
using MyVdsFactory.Application.Earthquakes.Commands.UpdateEarthquake;
using MyVdsFactory.Application.Earthquakes.Queries.GetEarthquake;
using MyVdsFactory.Application.Earthquakes.Queries.GetEarthquakeList;

namespace MyVdsFactory.API.Controllers
{
    public class EarthquakeController : BaseController
    {
        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<GetEarthquakeVm>> Get(long id)
        {
            return Ok(await Mediator.Send(new GetEarthquakeQuery
            {
                Id = id
            }));
        }

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<GetEarthquakeListVm>> GetAll([FromQuery] GetEarthquakeListRequestModel model)
        {
            return Ok(await Mediator.Send(new GetEarthquakeListQuery
            {
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Date = model.Date,
                Rms = model.Rms,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Magnitude = model.Magnitude,
                Location = model.Location,
                Country = model.Country,
                Province = model.Province,
                District = model.District
            }));
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(AddEarthquakeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        [Route("addExcel")]
        public async Task<IActionResult> AddExcel(IFormFile excelFile)
        {
            return Ok(await Mediator.Send(new AddEarthquakeWithExcelCommand
            {
                ExcelFile = excelFile
            }));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(UpdateEarthquakeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete(DeleteEarthquakeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}