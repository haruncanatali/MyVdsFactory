using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Prayers.Queries.Dtos;

namespace MyVdsFactory.Application.Prayers.Queries.GetPrayerList;

public class GetPrayerListQueryHandler : IRequestHandler<GetPrayerListQuery,GetPrayerListVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPrayerListQueryHandler> _logger;

    public GetPrayerListQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetPrayerListQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetPrayerListVm> Handle(GetPrayerListQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Prayers.Where(c => c.Date.Date.Month == request.Month &&
                                                       c.Date.Date.Year == request.Year &&
                                                       c.CityId == request.CityId)
            .OrderBy(c=>c.Date)
            .ProjectTo<PrayerDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Namaz vakti verisi listesi çekme girişimi.");
        
        return new GetPrayerListVm
        {
            Prayers = result,
            Count = result.Count
        };
    }
}