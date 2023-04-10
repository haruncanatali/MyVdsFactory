using HtmlAgilityPack;
using MediatR;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;

namespace MyVdsFactory.Application.Tarots.Commands.AddTarotWithHtml;

public class AddTarotWithHtmlCommand : IRequest<Result<long>>
{
    public class Handler : IRequestHandler<AddTarotWithHtmlCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<AddTarotWithHtmlCommand> _logger;

        public Handler(IApplicationContext context, ILogger<AddTarotWithHtmlCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(AddTarotWithHtmlCommand request, CancellationToken cancellationToken)
        {
            using (HttpClient client = new HttpClient())
            {
                HtmlDocument pageDocument;
                var response = await client.GetAsync(
                    $"https://www.boxerdergisi.com.tr/tarot-kartlarinin-anlamlari");
                var pageContents = await response.Content.ReadAsStringAsync(cancellationToken);
                    
                _logger.LogInformation("Tarot verisi isimlendirmeleri için url e istek atıldı.");
                    
                pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContents);

                var commentary = pageDocument.DocumentNode?.SelectSingleNode("/html[1]/body[1]/div[10]/div[1]/div[1]/div[4]")
                    ?.InnerText?.Replace("\n","");

                return null;
            }
        }
    }
}