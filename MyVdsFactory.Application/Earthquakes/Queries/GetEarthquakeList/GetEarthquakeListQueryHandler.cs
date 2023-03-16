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
            var sort = (request.SortBy == null || request.SortBy == "ASC") ? "ASC" : "DESC";
            
            if (request.Date != null)
            {
                eartquakeQuery = eartquakeQuery.Where(c => c.Date.Date == request.Date.Value.Date);
            }
            else if(request.StartTime != null && request.EndTime != null)
            {
                var s_Day = request.StartTime.Value.Date.Day;
                var s_Month = request.StartTime.Value.Date.Month;
                var s_Year = request.StartTime.Value.Date.Year;

                var e_Day = request.EndTime.Value.Date.Day;
                var e_Month = request.EndTime.Value.Date.Month;
                var e_Year = request.EndTime.Value.Date.Year;
                
                eartquakeQuery = eartquakeQuery.Where(c => 
                    c.Day >= s_Day && c.Month >= s_Month && c.Year >= s_Year &&
                    c.Day <= e_Day && c.Month <= e_Month && c.Year <= e_Year);
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
                eartquakeQuery = eartquakeQuery.Where(c => c.Location.ToLower() == request.Location.ToLower());
            }

            if (request.Country != null)
            {
                eartquakeQuery = eartquakeQuery.Where(c => c.Country.ToLower() == request.Country.ToLower());
            }

            if (request.Province != null)
            {
                eartquakeQuery = eartquakeQuery.Where(c => c.Province.ToLower() == request.Province.ToLower());
            }

            if (request.District != null)
            {
                eartquakeQuery = eartquakeQuery.Where(c => c.District.ToLower() == request.District.ToLower());
            }

            eartquakeQuery = sort == "ASC" ? eartquakeQuery.OrderBy(c => c.Date) : eartquakeQuery.OrderByDescending(c => c.Date);

            var result = await eartquakeQuery.ProjectTo<EarthquakeDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

            return new GetEarthquakeListVm
            {
                Earthquakes = result,
                Count = result.Count
            };
        }
    }
}
