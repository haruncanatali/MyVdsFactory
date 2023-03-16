using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Districts.Queries.Dtos;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Districts.Queries.GetDistrictList;

public class GetDistrictListQueryHandler : IRequestHandler<GetDistrictListQuery,GetDistrictListVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;

    public GetDistrictListQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetDistrictListVm> Handle(GetDistrictListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Districts.Include(c => c.City) as IQueryable<District>;

        if (request.CityId != 0)
        {
            query = query.Where(c => c.CityId == request.CityId);
        }

        if (request.CityName.IsNullOrEmpty().Equals(false))
        {
            query = query.Where(c => c.City.Name.ToLower() == request.CityName.ToLower());
        }

        var result = await query.ProjectTo<DistrictDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        var count = result.Count;

        return new GetDistrictListVm
        {
            Districts = result,
            Count = count
        };
    }
}