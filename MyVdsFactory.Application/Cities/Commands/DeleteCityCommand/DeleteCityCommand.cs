using MediatR;
using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Application.Cities.Commands.DeleteCityCommand;

public class DeleteCityCommand : IRequest<Result<long>>
{
    public long Id { get; set; }
    public class Handler : IRequestHandler<DeleteCityCommand, Result<long>>
    {
        private readonly IApplicationContext _context;

        public Handler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Result<long>> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            var city = await _context.Cities.SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (city == null)
            {
                return Result<long>.Failure(new List<string>{"Silinecek şehir bulunamadı."});
            }

            city.Status = EntityStatus.Passive;
            
            return Result<long>.Success(1,"Şehir başarıyla silindi");
        }
    }
}