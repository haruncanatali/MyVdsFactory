using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Prayers.Queries.Dtos;

namespace MyVdsFactory.Application.Prayers.Queries.GetPrayer;

public class GetPrayerQueryHandler : IRequestHandler<GetPrayerQuery,GetPrayerVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPrayerQueryHandler> _logger;

    public GetPrayerQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetPrayerQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetPrayerVm> Handle(GetPrayerQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Prayers
            .Where(c => c.CityId == request.CityId && 
                        c.Date.Date == request.Date.Date)
            .ProjectTo<PrayerDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        _logger.LogInformation("Namaz vakti tekil veri çekme girişimi");
        
        return new GetPrayerVm
        {
            Prayer = result
        };
    }
}