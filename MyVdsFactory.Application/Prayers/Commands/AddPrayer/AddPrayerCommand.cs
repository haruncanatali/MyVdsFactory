using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Prayers.Commands.AddPrayer;

public class AddPrayerCommand : IRequest<Result<long>>
{
    public string Fajr { get; set; }
    public string Tulu { get; set; }
    public string Zuhr { get; set; }
    public string Asr { get; set; }
    public string Magrib { get; set; }
    public string Isha { get; set; }
    public DateTime Date { get; set; }
    public long CityId { get; set; }
    
    public class Handler : IRequestHandler<AddPrayerCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<AddPrayerCommand> _logger;

        public Handler(IApplicationContext context, ILogger<AddPrayerCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(AddPrayerCommand request, CancellationToken cancellationToken)
        {
            var entityCount = await _context.Prayers.CountAsync(cancellationToken);

            if (entityCount == 27740)
            {
                _logger.LogInformation("Bu yılın verileri veritabanında eklenmiş.");
                return Result<long>.Failure(new List<string>{"Bu yılın verileri veritabanında eklenmiş."});
            }
            
            var identityResult = await _context.Prayers
                .FirstOrDefaultAsync(c => c.CityId == request.CityId && 
                                          c.Date.Date == request.Date.Date);

            _logger.LogInformation("Namaz verisi için mükerrerlik kontrolü yapıldı.");
            
            if (identityResult == null)
            {
                await _context.Prayers.AddAsync(new Prayer
                {
                    Fajr = request.Fajr,
                    Tulu = request.Tulu,
                    Zuhr = request.Zuhr,
                    Asr = request.Asr,
                    Maghrib = request.Magrib,
                    Isha = request.Isha,
                    CityId = request.CityId,
                    Date = request.Date
                }, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation("Namaz verisi kaydı yapıldı.");
                
                return Result<long>.Success(1,"Namaz vakti verisi başarıyla eklendi.");
            }
            
            _logger.LogInformation("Namaz verisi için mükerrerlik kontrolü başarısız oldu. Aynı tarihli veri var.");
            
            return Result<long>.Failure(new List<string>{"Aynı tarihli veri var."});
        }
    }
}