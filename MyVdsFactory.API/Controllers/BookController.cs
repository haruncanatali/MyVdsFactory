using Microsoft.AspNetCore.Mvc;
using MyVdsFactory.API.Models;
using MyVdsFactory.API.Services;
using MyVdsFactory.Application.Books.Commands.AddBookWithExcel;
using MyVdsFactory.Application.Books.Queries.GetBooksWithAuthor;
using MyVdsFactory.Application.Books.Queries.GetBooksWithChallenge;
using MyVdsFactory.Application.Books.Queries.GetRandomBook;

namespace MyVdsFactory.API.Controllers;

public class BookController : BaseController
{
    private readonly IFileServices _fileServices;

    public BookController(IFileServices fileServices)
    {
        _fileServices = fileServices;
    }
    
    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<GetBooksWithAuthorVm>> GetAll([FromQuery] string? authorName)
    {
        return Ok(await Mediator.Send(new GetBooksWithAuthorQuery
        {
            AuthorName = authorName
        }));
    }
    
    [HttpGet]
    [Route("getRandomBook")]
    public async Task<ActionResult<GetRandomBookVm>> GetRandomBook()
    {
        return Ok(await Mediator.Send(new GetRandomBookQuery()));
    }
    
    [HttpGet]
    [Route("getRandomBooksWithChallenge")]
    public async Task<ActionResult<GetRandomBookVm>> GetRandomBookWithChallenge()
    {
        return Ok(await Mediator.Send(new GetRandomBooksWithChallengeQuery()));
    }

    [HttpPost]
    [Route("addExcel")]
    public async Task<IActionResult> Add(IFormFile file)
    {
        bool uploadResult = await _fileServices.SaveFileAsync(file, ModalPaths.Book);

        if (uploadResult.Equals(true))
        {
            return Ok(await Mediator.Send(new AddBookWithExcelCommand
            {
                ExcelDataFile = file
            }));
        }

        return BadRequest();
    }
}