using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;

namespace MyVdsFactory.Application.Earthquakes.Commands.UpdateEarthquake
{
    public class UpdateEarthquakeCommand : IRequest<Result<long>>
    {
        public long Id { get; set; }
        public double Rms { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Magnitude { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public DateTime Date { get; set; }

        public class Handler : IRequestHandler<UpdateEarthquakeCommand, Result<long>>
        {
            private readonly IApplicationContext _context;

            public Handler(IApplicationContext context)
            {
                _context = context;
            }

            public async Task<Result<long>> Handle(UpdateEarthquakeCommand request, CancellationToken cancellationToken)
            {
                var earlyResult = await _context.Earthquakes.SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

                if (earlyResult == null)
                {
                    return Result<long>.Failure(new List<string>{"Deprem verisi veritabanında bulnamadı."});
                }

                earlyResult.Rms = request.Rms;
                earlyResult.Latitude = request.Latitude;
                earlyResult.Longitude = request.Longitude;
                earlyResult.Magnitude = request.Magnitude;
                earlyResult.Location = request.Location;
                earlyResult.Country = request.Country;
                earlyResult.Province = request.Province;
                earlyResult.District = request.District;
                earlyResult.Date = earlyResult.Date;

                await _context.SaveChangesAsync(cancellationToken);

                return Result<long>.Success(1,"Deprem verisi veritabanında başarıyla güncellendi.");
            }
        }
    }
}
