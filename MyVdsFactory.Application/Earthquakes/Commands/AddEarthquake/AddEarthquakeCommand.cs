using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Earthquakes.Commands.AddEarthquake
{
    public class AddEarthquakeCommand : IRequest<Result<long>>
    {
        public decimal Depth { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Magnitude { get; set; }
        public string Location { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }


        public class Handler : IRequestHandler<AddEarthquakeCommand, Result<long>>
        {
            private readonly IApplicationContext _context;

            public Handler(IApplicationContext context)
            {
                _context = context;
            }

            public async Task<Result<long>> Handle(AddEarthquakeCommand request, CancellationToken cancellationToken)
            {
                await _context.Earthquakes.AddAsync(new Earthquake
                {
                    Depth = request.Depth,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Magnitude = request.Magnitude,
                    Location = request.Location,
                    Province = request.Province,
                    District = request.District,
                    Type = request.Type,
                    Date = request.Date,
                    Year = request.Date.Date.Year,
                    Month = request.Date.Date.Month,
                    Day = request.Date.Date.Day
                }, cancellationToken);

                return Result<long>.Success(1,"Deprem verisi başarıyla eklendi.");
            }
        }
    }
}
