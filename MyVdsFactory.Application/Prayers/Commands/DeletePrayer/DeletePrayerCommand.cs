using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Application.Prayers.Commands.DeletePrayer;

public class DeletePrayerCommand : IRequest<Result<long>>
{
    public long Id { get; set; }
    public class Handler : IRequestHandler<DeletePrayerCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<DeletePrayerCommand> _logger;

        public Handler(IApplicationContext context, ILogger<DeletePrayerCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(DeletePrayerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Prayers.FirstOrDefaultAsync(c => c.Id == request.Id,cancellationToken);

            if (entity != null)
            {
                entity.Status = EntityStatus.Archived;
                await _context.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation("Namaz vakti verisi başarıyla silindi.");
                
                return Result<long>.Success(1,"Namaz vakti verisi başarıyla silindi.");
            }
            
            _logger.LogInformation("Namaz vakti verisi silinemedi.");
                
            return Result<long>.Failure(new List<string>{"Namaz vakti verisi başarıyla silindi."});
        }
    }
}