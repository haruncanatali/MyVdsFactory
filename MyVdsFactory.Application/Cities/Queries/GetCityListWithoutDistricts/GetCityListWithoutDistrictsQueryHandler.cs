using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyVdsFactory.Application.Cities.Queries.Dtos;
using MyVdsFactory.Application.Common.Interfaces;

namespace MyVdsFactory.Application.Cities.Queries.GetCityListWithoutDistricts;

public class GetCityListWithoutDistrictsQueryHandler : IRequestHandler<GetCityListWithoutDistrictsQuery,GetCityListWithoutDistrictsVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCityListWithoutDistrictsQueryHandler> _logger;

    public GetCityListWithoutDistrictsQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetCityListWithoutDistrictsQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetCityListWithoutDistrictsVm> Handle(GetCityListWithoutDistrictsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Cities.ProjectTo<CityWithoutDistrictDto>(_mapper.ConfigurationProvider);

        if (request.Name.IsNullOrEmpty().Equals(false))
        {
            query = query.Where(c => c.Name.Contains(request.Name));
        }

        var result = await query.ToListAsync(cancellationToken);

        return new GetCityListWithoutDistrictsVm
        {
            Cities = result,
            Count = result.Count
        };
    }
}