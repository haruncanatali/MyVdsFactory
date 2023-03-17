using MediatR;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Cities.Commands.AddCityCommand;

public class AddCityCommand : IRequest<Result<long>>
{
    public string Name { get; set; }
    public class Handler : IRequestHandler<AddCityCommand, Result<long>>
    {
        private readonly IApplicationContext _context;

        public Handler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Result<long>> Handle(AddCityCommand request, CancellationToken cancellationToken)
        {
            await _context.Cities.AddAsync(new City { Name = request.Name }, cancellationToken);
            return Result<long>.Success(1,"Şehir başarıyla eklendi.");
        }
    }
}