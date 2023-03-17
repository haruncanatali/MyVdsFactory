using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Application.Cities.Queries.Dtos;
using MyVdsFactory.Application.Common.Interfaces;

namespace MyVdsFactory.Application.Cities.Queries.GetCity;

public class GetCityQueryHandler : IRequestHandler<GetCityQuery,GetCityVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;

    public GetCityQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetCityVm> Handle(GetCityQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Cities.Include(c => c.Districts)
            .ProjectTo<CityDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        return new GetCityVm
        {
            City = result
        };
    }
}