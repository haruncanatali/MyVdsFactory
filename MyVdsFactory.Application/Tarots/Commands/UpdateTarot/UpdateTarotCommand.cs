using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;

namespace MyVdsFactory.Application.Tarots.Commands.UpdateTarot;

public class UpdateTarotCommand : IRequest<Result<long>>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Features { get; set; }
    public string PhotoUrl { get; set; }

    public class Handler : IRequestHandler<UpdateTarotCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<UpdateTarotCommand> _logger;

        public Handler(IApplicationContext context, ILogger<UpdateTarotCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(UpdateTarotCommand request, CancellationToken cancellationToken)
        {
            var entityResult = await _context.Tarots.FirstOrDefaultAsync(c => c.Id == request.Id,cancellationToken);

            if (entityResult != null)
            {

                entityResult.Name = request.Name;
                entityResult.Description = request.Description;
                entityResult.Features = request.Features;
                entityResult.PhotoUrl = request.PhotoUrl;
                
                await _context.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation($"Tarot verisi guncellendi.(ID:{entityResult.Id})");
                
                return Result<long>.Success(1,"Tarot verisi basariyla silindi.");
            }
            else
            {
                _logger.LogError("Tarot verisi guncellenemedi.(ID:bulunamadi!)");
                return Result<long>.Failure(new List<string>{"Tarot verisi guncellenemedi.(ID:bulunamadi!)"});
            }
        }
    }
}