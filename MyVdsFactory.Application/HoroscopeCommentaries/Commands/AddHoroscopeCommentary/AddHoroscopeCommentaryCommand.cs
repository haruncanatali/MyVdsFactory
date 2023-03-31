using MediatR;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.HoroscopeCommentaries.Commands.AddHoroscopeCommentary;

public class AddHoroscopeCommentaryCommand : IRequest<Result<long>>
{
    public string Commentary { get; set; }
    public DateTime Date { get; set; }
    public long HoroscopeId { get; set; }
    
    public class Handler : IRequestHandler<AddHoroscopeCommentaryCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<AddHoroscopeCommentaryCommand> _logger;
        
        public async Task<Result<long>> Handle(AddHoroscopeCommentaryCommand request, CancellationToken cancellationToken)
        {
            var horoscopeCommentary = new HoroscopeCommentary
            {
                Commentary = request.Commentary,
                Date = request.Date,
                HoroscopeId = request.HoroscopeId
            };

            await _context.HoroscopeCommentaries.AddAsync(horoscopeCommentary, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Burç yorumu ekleme girişimi ID:"+horoscopeCommentary.Id);
            
            return Result<long>.Success(1,"Burç yorumu ekleme işlemi başarılı oldu.");
        }
    }
}