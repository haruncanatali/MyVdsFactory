using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Application.Earthquakes.Commands.DeleteEarthquake
{
    public class DeleteEarthquakeCommand : IRequest<Result<long>>
    {
        public long Id { get; set; }

        public class Handler : IRequestHandler<DeleteEarthquakeCommand, Result<long>>
        {
            private IApplicationContext _context;

            public Handler(IApplicationContext context)
            {
                _context = context;
            }

            public async Task<Result<long>> Handle(DeleteEarthquakeCommand request, CancellationToken cancellationToken)
            {
                var data = await _context.Earthquakes.SingleOrDefaultAsync(c => c.Id == request.Id,cancellationToken);

                if (data == null)
                {
                    return Result<long>.Failure(new List<string>{"Deprem verisi veritabanında bulunamadı."});
                }

                data.Status = EntityStatus.Passive;

                await _context.SaveChangesAsync(cancellationToken);

                return Result<long>.Success(1,"Deprem verisi veritabanında pasifize edildi.");
            }
        }
    }
}
