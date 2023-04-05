using System.Globalization;
using AutoMapper;
using HtmlAgilityPack;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Earthquakes.Commands.AddEarthquakeWithHtml;

public class AddEarthquakeWithHtmlCommand : IRequest<Result<long>>
{
    public class Handler : IRequestHandler<AddEarthquakeWithHtmlCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AddEarthquakeWithHtmlCommand> _logger;

        public Handler(IApplicationContext context, IMapper mapper, ILogger<AddEarthquakeWithHtmlCommand> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(AddEarthquakeWithHtmlCommand request, CancellationToken cancellationToken)
        {
            using (HttpClient client = new HttpClient())
            {
                HtmlDocument pageDocument = new HtmlDocument();

                var response = await client.GetAsync("https://deprem.afad.gov.tr/last-earthquakes.html");
                var pageContents = await response.Content.ReadAsStringAsync(cancellationToken);
                pageDocument.LoadHtml(pageContents);
                
                var tbodyHtml = pageDocument.DocumentNode?.SelectSingleNode("/html[1]/body[1]/div[3]/table[1]/tbody[1]");
                List<Earthquake> earthquakes = new List<Earthquake>();

                foreach (var tr in tbodyHtml.ChildNodes)
                {
                    var date = DateTime.Parse(tr.ChildNodes[0].InnerText);
                    var year = date.Year;
                    var month = date.Month;
                    var day = date.Day;

                    decimal latitude = 0.0m;
                    decimal longitude = 0.0m;
                    decimal magnitude = 0.0m;
                    decimal depth = 0.0m;
                    
                    decimal.TryParse(tr.ChildNodes[1].InnerText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out latitude);
                    decimal.TryParse(tr.ChildNodes[2].InnerText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out longitude);
                    decimal.TryParse(tr.ChildNodes[3].InnerText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out depth);
                    decimal.TryParse(tr.ChildNodes[5].InnerText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out magnitude);
                    
                    var type = tr.ChildNodes[4].InnerText;
                    var location = tr.ChildNodes[6].InnerText;
                    var district = location.Split(' ')[0];
                    var province = location.Split(' ')[1]
                        .Replace(")","").Replace("(", "");

                    var referenceId = long.Parse(tr.ChildNodes[7].InnerText);
                    
                    earthquakes.Add(new Earthquake
                    {
                        Date = date,
                        Year = year,
                        Month = month,
                        Day = day,
                        Latitude = latitude,
                        Longitude = longitude,
                        Magnitude = magnitude,
                        Depth = depth,
                        Type = type,
                        Location = location,
                        District = district,
                        Province = province,
                        ReferenceId = referenceId
                    });
                }

                var lastData_db = await _context.Earthquakes.OrderByDescending(c => c.ReferenceId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (lastData_db != null && earthquakes.Count>0)
                {
                    var lastData_source = earthquakes.OrderByDescending(c=>c.ReferenceId).First();
                    
                    if (lastData_db.ReferenceId == lastData_source.ReferenceId)
                    {
                        return Result<long>.Failure(new List<string>{"Deprem veri kaynağında değişiklik yok."});
                    }

                    List<Earthquake> result = new List<Earthquake>();
                    
                    earthquakes.ForEach(c =>
                    {
                        if (c.ReferenceId > lastData_db.ReferenceId)
                        {
                            result.Add(c);
                        }
                    });

                    await _context.Earthquakes.AddRangeAsync(result, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    
                    return Result<long>.Success(1,"Deprem verileri başarıyla eklendi.");
                }
                else if (lastData_db == null && earthquakes.Count > 0)
                {
                    await _context.Earthquakes.AddRangeAsync(earthquakes, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    
                    return Result<long>.Success(1,"Deprem verileri başarıyla eklendi.");
                }
                
                
                return Result<long>.Failure(new List<string>{"Deprem verileri eklenemedi."});
            }
        }
    }
}