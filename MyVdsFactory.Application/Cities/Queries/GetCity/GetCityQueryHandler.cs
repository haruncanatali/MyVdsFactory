using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Cities.Queries.Dtos;
using MyVdsFactory.Application.Common.Interfaces;

namespace MyVdsFactory.Application.Cities.Queries.GetCity;

public class GetCityQueryHandler : IRequestHandler<GetCityQuery,GetCityVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCityQueryHandler> _logger;

    public GetCityQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetCityQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetCityVm> Handle(GetCityQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Cities.Include(c => c.Districts)
            .ProjectTo<CityDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        
        _logger.LogInformation($"{(result == null ? "Ilgili id li sehir bulunamadi" : result.CityName+" sehri veritabanindan ilceleri ile beraber sorgulandi.")}");
        
        return new GetCityVm
        {
            City = result
        };
    }
}