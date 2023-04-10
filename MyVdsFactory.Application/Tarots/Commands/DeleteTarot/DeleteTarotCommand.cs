using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Application.Tarots.Commands.DeleteTarot;

public class DeleteTarotCommand : IRequest<Result<long>>
{
    public long Id { get; set; }

    public class Handler : IRequestHandler<DeleteTarotCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<DeleteTarotCommand> _logger;

        public Handler(IApplicationContext context, ILogger<DeleteTarotCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(DeleteTarotCommand request, CancellationToken cancellationToken)
        {
            var entityResult = await _context.Tarots.FirstOrDefaultAsync(c => c.Id == request.Id,cancellationToken);

            if (entityResult != null)
            {
                entityResult.Status = EntityStatus.Archived;
                await _context.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation($"Tarot verisi arsivlendi.(ID:{entityResult.Id})");
                
                return Result<long>.Success(1,"Tarot verisi basariyla silindi.");
            }
            else
            {
                _logger.LogError("Tarot verisi arsivlenemedi.(ID:bulunamadi!)");
                return Result<long>.Failure(new List<string>{"Tarot verisi arsivlenemedi.(ID:bulunamadi!)"});
            }
        }
    }
}