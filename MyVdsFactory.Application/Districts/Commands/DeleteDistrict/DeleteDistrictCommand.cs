using MediatR;
using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Application.Districts.Commands.DeleteDistrict;

public class DeleteDistrictCommand : IRequest<Result<long>>
{
    public long DistrictId { get; set; }

    public class Handler : IRequestHandler<DeleteDistrictCommand, Result<long>>
    {
        private readonly IApplicationContext _context;

        public Handler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Result<long>> Handle(DeleteDistrictCommand request, CancellationToken cancellationToken)
        {
            var district = await _context.Districts
                .SingleOrDefaultAsync(c => c.Id == request.DistrictId, cancellationToken: cancellationToken);
            if (district == null)
            {
                return Result<long>.Failure(new List<string>{"İlçe bulunamadı."});
            }

            district.Status = EntityStatus.Passive;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Result<long>.Success(1,"İlçe başarıyla silindi.");
        }
    }
}