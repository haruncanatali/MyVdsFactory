using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Extensions;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;

namespace MyVdsFactory.Application.Horoscopes.Commands.UpdateHoroscopeCommand;

public class UpdateHoroscopeCommand : IRequest<Result<long>>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string PhotoName { get; set; }
    public string DateRange { get; set; }
    public string Planet { get; set; }
    public string Description { get; set; }
    public string Group { get; set; }

    public class Handler : IRequestHandler<UpdateHoroscopeCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<UpdateHoroscopeCommand> _logger;

        public Handler(IApplicationContext context, ILogger<UpdateHoroscopeCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(UpdateHoroscopeCommand request, CancellationToken cancellationToken)
        {
            var horoscope = await _context.Horoscopes.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (horoscope == null)
            {
                _logger.LogError("Burç güncelleme girişimi : "+request.Id+" id li entity bulunamadı!");
                return Result<long>.Failure(new List<string>{"Güncellenecek burç bulunamadı."});
            }

            horoscope.Name = request.Name;
            horoscope.PhotoName = request.PhotoName;
            horoscope.DateRange = request.DateRange;
            horoscope.NormalizedName = request.Name.ReplaceTurkishCharacters();
            horoscope.Description = request.Description;
            horoscope.Group = request.Group;
            horoscope.Planet = request.Planet;

            _context.Horoscopes.Update(horoscope);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Result<long>.Success(1,"Burç başarıyla güncellendi.");
        }
    }
}