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
        public double Rms { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Magnitude { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
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
                    Rms = request.Rms,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Magnitude = request.Magnitude,
                    Location = request.Location,
                    Country = request.Country,
                    Province = request.Province,
                    District = request.District,
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
