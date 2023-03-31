using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Application.Horoscopes.Commands.DeleteHoroscopeCommand;

public class DeleteHoroscopeCommand : IRequest<Result<long>>
{
    public long Id { get; set; }

    public class Handler : IRequestHandler<DeleteHoroscopeCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<DeleteHoroscopeCommand> _logger;

        public Handler(IApplicationContext context, ILogger<DeleteHoroscopeCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(DeleteHoroscopeCommand request, CancellationToken cancellationToken)
        {
            var horoscope = await _context.Horoscopes.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (horoscope == null)
            {
                _logger.LogError("Burç silme girişimi : "+request.Id+" id li entity bulunamadı!");
                return Result<long>.Failure(new List<string>{"Silinecek burç bulunamadı."});
            }

            horoscope.Status = EntityStatus.Archived;
            _context.Horoscopes.Update(horoscope);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Burç silme girişimi : "+request.Id+" id li entity silindi!");
            return Result<long>.Success(1,"Burç başarıyla silindi.");
        }
    }
}