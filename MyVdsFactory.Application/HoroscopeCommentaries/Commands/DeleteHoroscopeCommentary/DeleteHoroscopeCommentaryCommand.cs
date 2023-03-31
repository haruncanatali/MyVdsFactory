using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Application.Horoscopes.Commands.DeleteHoroscopeCommand;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Application.HoroscopeCommentaries.Commands.DeleteHoroscopeCommentary;

public class DeleteHoroscopeCommentaryCommand : IRequest<Result<long>>
{
    public long Id { get; set; }

    public class Handler : IRequestHandler<DeleteHoroscopeCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<DeleteHoroscopeCommentaryCommand> _logger;

        public Handler(IApplicationContext context, ILogger<DeleteHoroscopeCommentaryCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(DeleteHoroscopeCommand request, CancellationToken cancellationToken)
        {
            var horoscopeCommentary = await _context.HoroscopeCommentaries.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (horoscopeCommentary == null)
            {
                _logger.LogError("Burç yorumu silme girişimi : "+request.Id+" id li entity bulunamadı!");
                return Result<long>.Failure(new List<string>{"Silinecek burç yorumu bulunamadı."});
            }

            horoscopeCommentary.Status = EntityStatus.Archived;
            await _context.SaveChangesAsync(cancellationToken);
            
            return Result<long>.Success(1,"Burç yorumu başarıyla silindi.");
        }
    }
}