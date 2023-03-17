using MediatR;
using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;

namespace MyVdsFactory.Application.Cities.Commands.UpdateCityCommand;

public class UpdateCityCommand : IRequest<Result<long>>
{
    public long Id { get; set; }
    public string Name { get; set; }
    
    public class Handler : IRequestHandler<UpdateCityCommand, Result<long>>
    {
        private readonly IApplicationContext _context;

        public Handler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Result<long>> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            var city = await _context.Cities.SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (city == null)
            {
                return Result<long>.Failure(new List<string>{"Güncellenecek şehir bulunamadı."});
            }

            city.Name = request.Name;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Result<long>.Success(1,"Şehir başarıyla güncellendi.");
        }
    }
}