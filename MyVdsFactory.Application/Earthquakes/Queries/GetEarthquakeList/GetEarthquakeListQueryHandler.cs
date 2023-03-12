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
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Earthquakes.Queries.GetEarthquakeList
{
    internal class GetEarthquakeListQueryHandler : IRequestHandler<GetEarthquakeListQuery,GetEarthquakeListVm>
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;

        public GetEarthquakeListQueryHandler(IApplicationContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetEarthquakeListVm> Handle(GetEarthquakeListQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Earthquake> eartquakeQuery = _context.Earthquakes;

            
            if (request.Date != null)
            {
                eartquakeQuery = eartquakeQuery.Where(c => c.Date.Date == request.Date.Value.Date);
            }
            else if(request.StartTime != null && request.EndTime != null)
            {
                eartquakeQuery = eartquakeQuery.Where(c =>
                    c.Date.Date >= request.StartTime.Value.Date && c.Date.Date <= request.EndTime.Value.Date);
            }

            if (request.Rms != null)
            {
                eartquakeQuery = eartquakeQuery.Where(c => c.Rms == request.Rms);
            }

            if (request.Latitude != null)
            {
                eartquakeQuery = eartquakeQuery.Where(c => c.Latitude == request.Latitude);
            }

            if (request.Longitude != null)
            {
                eartquakeQuery = eartquakeQuery.Where(c => c.Longitude == request.Longitude);
            }

            if (request.Magnitude != null)
            {
                eartquakeQuery = eartquakeQuery.Where(c => c.Magnitude == request.Magnitude);
            }

            if (request.Location != null)
            {
                eartquakeQuery = eartquakeQuery.Where(c => String.Equals(c.Location, request.Location, StringComparison.CurrentCultureIgnoreCase));
            }

            if (request.Country != null)
            {
                eartquakeQuery = eartquakeQuery.Where(c => String.Equals(c.Country, request.Country, StringComparison.CurrentCultureIgnoreCase));
            }

            if (request.Province != null)
            {
                eartquakeQuery = eartquakeQuery.Where(c => String.Equals(c.Province, request.Province, StringComparison.CurrentCultureIgnoreCase));
            }

            if (request.District != null)
            {
                eartquakeQuery = eartquakeQuery.Where(c => String.Equals(c.District, request.District, StringComparison.CurrentCultureIgnoreCase));
            }

            var result = await eartquakeQuery.ProjectTo<EarthquakeDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

            return new GetEarthquakeListVm
            {
                Earthquakes = result,
                Count = result.Count
            };
        }
    }
}
