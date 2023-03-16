using MediatR;
using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;

namespace MyVdsFactory.Application.Districts.Commands.UpdateDistrict;

public class UpdateDistrictCommand : IRequest<Result<long>>
{
    public long DistrictId { get; set; }
    public long CityId { get; set; }
    public string Name { get; set; }

    public class Handler : IRequestHandler<UpdateDistrictCommand, Result<long>>
    {
        private readonly IApplicationContext _context;

        public Handler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Result<long>> Handle(UpdateDistrictCommand request, CancellationToken cancellationToken)
        {
            var district =
                await _context.Districts.SingleOrDefaultAsync(c => c.Id == request.DistrictId, cancellationToken);

            var city = await _context.Cities.SingleOrDefaultAsync(c => c.Id == request.CityId, cancellationToken);

            if (district == null)
            {
                return Result<long>.Failure(new List<string>{"ilçe bulunamadı."});
            }

            if (city == null)
            {
                return Result<long>.Failure(new List<string>{"İl bulunamadı."});
            }

            district.Name = request.Name;
            district.CityId = request.CityId;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Result<long>.Success(1,"İlçe başarıyla güncellendi.");
        }
    }
}