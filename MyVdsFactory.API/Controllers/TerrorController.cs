using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVdsFactory.API.Models;
using MyVdsFactory.API.Services;
using MyVdsFactory.Application.Cities.Commands.AddCityWithExcel;
using MyVdsFactory.Application.Terrors.Commands.AddTerrorWithExcelCommand;

namespace MyVdsFactory.API.Controllers;

public class TerrorController : BaseController
{
    private readonly IFileServices _fileService;

    public TerrorController(IFileServices fileService)
    {
        _fileService = fileService;
    }
    
    [HttpPost]
    [Route("addExcel")]
    [AllowAnonymous]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> AddExcel(IFormFile excelFile)
    {
        var copyResult = await _fileService.SaveFileAsync(excelFile, ModalPaths.Terror);

        if (copyResult)
        {
            return Ok(await Mediator.Send(new AddTerrorWithExcelCommand
            {
                ExcelDataFile = excelFile
            }));
        }

        return BadRequest("Dosya kaydedilemedi.");
    }
}