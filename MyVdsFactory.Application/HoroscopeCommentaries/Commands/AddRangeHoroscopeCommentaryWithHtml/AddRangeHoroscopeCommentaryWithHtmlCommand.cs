using AutoMapper;
using AutoMapper.QueryableExtensions;
using HtmlAgilityPack;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Application.Horoscopes.Queries.Dtos;
using MyVdsFactory.Domain.Entities;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Application.HoroscopeCommentaries.Commands.AddRangeHoroscopeCommentaryWithHtml;

public class AddRangeHoroscopeCommentaryWithHtmlCommand : IRequest<Result<long>>
{
    public class Handler : IRequestHandler<AddRangeHoroscopeCommentaryWithHtmlCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AddRangeHoroscopeCommentaryWithHtmlCommand> _logger;

        public Handler(IApplicationContext context, ILogger<AddRangeHoroscopeCommentaryWithHtmlCommand> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<long>> Handle(AddRangeHoroscopeCommentaryWithHtmlCommand request, CancellationToken cancellationToken)
        {

            var horoscopes = await _context.Horoscopes
                .ProjectTo<HoroscopeDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            
            _logger.LogInformation("Burç yorumları için burçlar çekildi.");
            
            using (HttpClient client = new HttpClient())
            {
                HtmlDocument pageDocument;
                foreach (var horoscope in horoscopes)
                {
                    var response = await client.GetAsync(
                        $"https://www.mynet.com/kadin/burclar-astroloji/{horoscope.NormalizedName}-burcu-gunluk-yorumu.html");
                    var pageContents = await response.Content.ReadAsStringAsync(cancellationToken);
                    
                    _logger.LogInformation("Burç verisi için url e istek atıldı.");
                    
                    pageDocument = new HtmlDocument();
                    pageDocument.LoadHtml(pageContents);

                    var commentary = pageDocument.DocumentNode?.SelectSingleNode("/html[1]/body[1]/div[10]/div[1]/div[1]/div[4]")
                        ?.InnerText?.Replace("\n","");

                    var horoscopeCommentary = new HoroscopeCommentary
                    {
                        Commentary = commentary ?? "HATA",
                        HoroscopeId = horoscope.Id,
                        Date = DateTime.Now
                    };

                    var oldCommentaries = await _context.HoroscopeCommentaries
                        .Where(c => c.HoroscopeId == horoscope.Id && c.Status == EntityStatus.Active)
                        .ToListAsync(cancellationToken);
                    
                    oldCommentaries.ForEach(c =>
                    {
                        c.Status = EntityStatus.Archived;
                    });
                    
                    _context.HoroscopeCommentaries.UpdateRange(oldCommentaries);
                    await _context.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation("Eski tarihli yorumlar arşivlendi.");
                    
                    await _context.HoroscopeCommentaries.AddAsync(horoscopeCommentary, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    
                    _logger.LogInformation("Yeni yorum eklendi.");
                }
            }
            
            return Result<long>.Success(1,"Yorumlar başarıyla eklendi.");
        }
        
    }
}