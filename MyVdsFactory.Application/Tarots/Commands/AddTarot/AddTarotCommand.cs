using MediatR;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Tarots.Commands.AddTarot;

public class AddTarotCommand : IRequest<Result<long>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Features { get; set; }
    public string PhotoUrl { get; set; }
    
    public class Handler : IRequestHandler<AddTarotCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<AddTarotCommand> _logger;

        public Handler(IApplicationContext context, ILogger<AddTarotCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(AddTarotCommand request, CancellationToken cancellationToken)
        {
            await _context.Tarots.AddAsync(new Tarot
            {
                Name = request.Name,
                Description = request.Description,
                Features = request.Features,
                PhotoUrl = request.PhotoUrl
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Tarot verisi basariyla eklendi.");
            
            return Result<long>.Success(1,"Tarot verisi basariyla eklendi.");
        }
    }
}