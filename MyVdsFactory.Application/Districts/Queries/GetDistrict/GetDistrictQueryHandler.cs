using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Districts.Queries.Dtos;

namespace MyVdsFactory.Application.Districts.Queries.GetDistrict;

public class GetDistrictQueryHandler : IRequestHandler<GetDistrictQuery,GetDistrictVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;

    public GetDistrictQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetDistrictVm> Handle(GetDistrictQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Districts
            .Include(c=>c.City)
            .ProjectTo<DistrictDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(c => c.DistrictId == request.Id,cancellationToken);

        return new GetDistrictVm
        {
            District = result
        };
    }
}