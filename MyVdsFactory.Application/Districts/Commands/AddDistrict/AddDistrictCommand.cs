using MediatR;
using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Districts.Commands.AddDistrict;

public class AddDistrictCommand : IRequest<Result<long>>
{
    public string Name { get; set; }
    public long CityId { get; set; }
    public class Handler : IRequestHandler<AddDistrictCommand, Result<long>>
    {
        private readonly IApplicationContext _context;

        public Handler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Result<long>> Handle(AddDistrictCommand request, CancellationToken cancellationToken)
        {
            var city = await _context.Districts
                .SingleOrDefaultAsync(c => c.Id == request.CityId, cancellationToken: cancellationToken);

            if (city == null)
            {
                return Result<long>.Failure(new List<string>{"Şehir bulunamadı."});
            }

            await _context.Districts.AddAsync(new District
            {
                Name = request.Name,
                CityId = request.CityId
            });

            await _context.SaveChangesAsync(cancellationToken);
            
            return Result<long>.Success(1,"İlçe başarıyla eklendi.");
        }
    }
}