using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyVdsFactory.Application.Cities.Queries.Dtos;
using MyVdsFactory.Application.Common.Interfaces;

namespace MyVdsFactory.Application.Cities.Queries.GetCityList;

public class GetCityListQueryHandler : IRequestHandler<GetCityListQuery,GetCityListVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;

    public GetCityListQueryHandler(IMapper mapper, IApplicationContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetCityListVm> Handle(GetCityListQuery request, CancellationToken cancellationToken)
    {
        var result =  request.Name.IsNullOrEmpty().Equals(false) ? await _context.Cities.Include(c => c.Districts)
            .ProjectTo<CityDto>(_mapper.ConfigurationProvider)
            .Where(c=>c.CityName.ToLower() == request.Name!.ToLower())
            .ToListAsync(cancellationToken)
            : await _context.Cities.Include(c => c.Districts)
                .ProjectTo<CityDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        var count = result.Count;

        return new GetCityListVm
        {
            Cities = result,
            Count = count
        };
    }
}