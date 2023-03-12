using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Earthquakes.Queries.Dtos;

namespace MyVdsFactory.Application.Earthquakes.Queries.GetEarthquake
{
    public class GetEarthquakeQueryHandler : IRequestHandler<GetEarthquakeQuery,GetEarthquakeVm>
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;

        public GetEarthquakeQueryHandler(IApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetEarthquakeVm> Handle(GetEarthquakeQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Earthquakes.ProjectTo<EarthquakeDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            return new GetEarthquakeVm
            {
                Earthquake = result
            };
        }
    }
}
