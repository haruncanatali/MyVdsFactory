using MediatR;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Cities.Commands.AddCityWithDistrictsCommand;

public class AddCityWithDistrictsCommand : IRequest<Result<long>>
{
    public string Name { get; set; }
    public List<string> Districts { get; set; }

    public class Handler : IRequestHandler<AddCityWithDistrictsCommand, Result<long>>
    {
        private readonly IApplicationContext _context;

        public Handler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Result<long>> Handle(AddCityWithDistrictsCommand request, CancellationToken cancellationToken)
        {
            var entityResult = await _context.Cities.AddAsync(new City { Name = request.Name }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            List<District> districts = request.Districts.Select(c => new District
            {
                Name = c,
                CityId = entityResult.Entity.Id
            }).ToList();
            await _context.Districts.AddRangeAsync(districts, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<long>.Success(1,"Şehir ilçeleriyle beraber başarıyla eklendi.");
        }
    }
}