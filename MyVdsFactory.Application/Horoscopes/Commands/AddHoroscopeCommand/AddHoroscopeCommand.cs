using MediatR;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Extensions;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Horoscopes.Commands.AddHoroscopeCommand;

public class AddHoroscopeCommand : IRequest<Result<long>>
{
    public string Name { get; set; }
    public string PhotoName { get; set; }
    public string DateRange { get; set; }
    public string Planet { get; set; }
    public string Description { get; set; }
    public string Group { get; set; }

    public class Handler : IRequestHandler<AddHoroscopeCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<AddHoroscopeCommand> _logger;

        public Handler(IApplicationContext context, ILogger<AddHoroscopeCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(AddHoroscopeCommand request, CancellationToken cancellationToken)
        {
            var horoscope = new Horoscope
            {
                Name = request.Name,
                PhotoName = request.PhotoName,
                DateRange = request.DateRange,
                NormalizedName = request.Name.ReplaceTurkishCharacters(),
                Planet = request.Planet,
                Group = request.Group,
                Description = request.Description
            };
            
            await _context.Horoscopes.AddAsync(horoscope, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Burç ekleme girişimi - ID:"+horoscope.Id);
            
            return Result<long>.Success(1,"Burç başarıyla eklendi.");
        }
    }
}