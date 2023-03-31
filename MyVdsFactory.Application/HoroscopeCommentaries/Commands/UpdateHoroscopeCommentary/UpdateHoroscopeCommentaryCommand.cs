using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;

namespace MyVdsFactory.Application.HoroscopeCommentaries.Commands.UpdateHoroscopeCommentary;

public class UpdateHoroscopeCommentaryCommand : IRequest<Result<long>>
{
    public long Id { get; set; }
    public string Commentary { get; set; }
    public DateTime Date { get; set; }
    public long HoroscopeId { get; set; }

    public class Handler : IRequestHandler<UpdateHoroscopeCommentaryCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<UpdateHoroscopeCommentaryCommand> _logger;

        public Handler(IApplicationContext context, ILogger<UpdateHoroscopeCommentaryCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(UpdateHoroscopeCommentaryCommand request, CancellationToken cancellationToken)
        {
            var horoscopeCommentary = await _context.HoroscopeCommentaries.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (horoscopeCommentary == null)
            {
                _logger.LogError("Burç yorumu güncelleme girişimi : "+request.Id+" id li entity bulunamadı!");
                return Result<long>.Failure(new List<string>{"Güncellenecek burç yorumu bulunamadı."});
            }

            horoscopeCommentary.Commentary = request.Commentary;
            horoscopeCommentary.Date = request.Date;
            horoscopeCommentary.HoroscopeId = request.HoroscopeId;

            _context.HoroscopeCommentaries.Update(horoscopeCommentary);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Result<long>.Success(1,"Burç yorumu başarıyla güncellendi.");
        }
    }
}