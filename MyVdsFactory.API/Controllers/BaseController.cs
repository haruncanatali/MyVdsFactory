using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVdsFactory.API.Services;

namespace MyVdsFactory.API.Controllers;

[ApiController]
//[Authorize]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}