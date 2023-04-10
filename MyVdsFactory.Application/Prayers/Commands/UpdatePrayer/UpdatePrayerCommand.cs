using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;

namespace MyVdsFactory.Application.Prayers.Commands.UpdatePrayer;

public class UpdatePrayerCommand : IRequest<Result<long>>
{
    public string Fajr { get; set; }
    public string Tulu { get; set; }
    public string Zuhr { get; set; }
    public string Asr { get; set; }
    public string Magrib { get; set; }
    public string Isha { get; set; }
    public DateTime Date { get; set; }
    public long CityId { get; set; }
    public long Id { get; set; }
    
    public class Handler : IRequestHandler<UpdatePrayerCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<UpdatePrayerCommand> _logger;

        public Handler(IApplicationContext context, ILogger<UpdatePrayerCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(UpdatePrayerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Prayers.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            _logger.LogInformation("Namaz verisi için arama yapıldı.");
            
            if (entity != null)
            {
                entity.Fajr = request.Fajr;
                entity.Tulu = request.Tulu;
                entity.Zuhr = request.Zuhr;
                entity.Asr = request.Asr;
                entity.Maghrib = request.Magrib;
                entity.Isha = request.Isha;
                entity.Date = request.Date;
                entity.CityId = request.CityId;
                
                await _context.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation("Namaz verisi güncellendi.");
                return Result<long>.Success(1,"Namaz verisi güncellendi.");
            }
            
            _logger.LogInformation("Namaz verisi güncellenemedi.");
            return Result<long>.Failure(new List<string>{"Namaz verisi güncellenemedi."});
        }
    }
}