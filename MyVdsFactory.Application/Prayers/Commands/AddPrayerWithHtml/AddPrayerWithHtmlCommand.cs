using AutoMapper;
using HtmlAgilityPack;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyVdsFactory.Application.Common.Extensions;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Prayers.Commands.AddPrayerWithHtml;

public class AddPrayerWithHtmlCommand : IRequest<Result<long>>
{
    public class Handler : IRequestHandler<AddPrayerWithHtmlCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AddPrayerWithHtmlCommand> _logger;

        public Handler(IApplicationContext context, ILogger<AddPrayerWithHtmlCommand> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<long>> Handle(AddPrayerWithHtmlCommand request, CancellationToken cancellationToken)
        {
            var entityCount = await _context.Prayers.CountAsync(cancellationToken);
            var randomEntity = await _context.Prayers
                .FirstOrDefaultAsync(c => c.Id == (new Random().Next(16702, 18198)),cancellationToken);

            if (entityCount == 27740 && randomEntity?.Date.Date.Year == DateTime.Now.Year)
            {
                _logger.LogInformation("Bu yılın verileri veritabanında eklenmiş.");
                return Result<long>.Failure(new List<string>{"Bu yılın verileri veritabanında eklenmiş."});
            }
            else
            {
                await _context.Prayers.ExecuteDeleteAsync(cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation($"Veritabanındaki tüm namaz vakitleri verisi silindi.({DateTime.Now.Year})");
            }
            
            List<Prayer> prayers = new List<Prayer>();
            
            using (HttpClient client = new HttpClient())
            {
                for (int id = 16702; id <= 18197; id++)
                {
                    for (int month = 0; month < 12; month++)
                    {
                        HtmlDocument pageDocument;
                        var response = await client.GetAsync(
                            $"https://www.namazvakti.com/Print.php?cityID={id.ToString()}&selMonth={month.ToString()}&print=0");
                        var pageContents = await response.Content.ReadAsStringAsync(cancellationToken);
                        
                        _logger.LogInformation($"İstek atıldı. Id:{id} Month:{month}");
                
                        pageDocument = new HtmlDocument();
                        pageDocument.LoadHtml(pageContents);
                        var dailyPrayerTimes = pageDocument.DocumentNode?.SelectSingleNode("/html[1]/body[1]/div[1]/table[2]");

                        var cityProvinceText = pageDocument.DocumentNode?.SelectSingleNode(
                            "/html[1]/body[1]/div[1]/table[1]/tr[1]/td[1]/div[1]").InnerText.Trim();
                        var cityWebText = pageDocument.DocumentNode?.SelectSingleNode(
                            "/html[1]/body[1]/div[1]/table[1]/tr[1]/td[1]/div[2]").InnerText.Split('&')[0].Trim();
                        var countryWebText = pageDocument.DocumentNode?.SelectSingleNode(
                            "/html[1]/body[1]/div[1]/table[1]/tr[1]/td[1]/div[2]").InnerText.Split("&nbsp;")[2].Trim();

                        var cityDbResult = await _context.Cities
                            .FirstOrDefaultAsync(c => c.Name == cityWebText, cancellationToken);

                        if (cityDbResult != null && (countryWebText == "Türkiye" && cityProvinceText == cityWebText))
                        {
                            var nodes = dailyPrayerTimes.ChildNodes.Skip(3).ToList();
                            
                            _logger.LogInformation($"Şehir bulundu. ({cityDbResult.Name})");

                            foreach (var node in nodes)
                            {
                                var check = node.InnerText.Replace("\n\t\t", "").IsNullOrEmpty().Equals(true);
                                if (check.Equals(false))
                                {
                                    var value = node.InnerText.Replace("\n\t\t", "");
                                    var day = Convert.ToInt32(node.InnerText.Replace("\n\t\t", "")
                                        .Split('&')[0]);
                                    var split1 = value.Split(';')[1];
                                    var indexCounter = 0;
                                    for (int i = 0; i < split1.Length; i++)
                                    {
                                        if (Char.IsNumber(split1[i]))
                                        {
                                            indexCounter = i;
                                            break;
                                        }
                                    }

                                    var split2 = split1.Substring(indexCounter, split1.Length - indexCounter)
                                        .Replace("\n","")
                                        .Replace("\t","")
                                        .Replace(" ","");

                                    string Fajr = split2.Substring(0, 5);  // imsak
                                    string Tulu = split2.Substring(5, 5); // Gunes
                                    string Zuhr = split2.Substring(10, 5); // Ogle
                                    string Asr = split2.Substring(15, 5);// Ikindi
                                    string Maghrib = split2.Substring(20, 5); // Aksam
                                    string Isha = split2.Substring(25, 5); // yatsi
                                    
                                    prayers.Add(new Prayer
                                    {
                                        Fajr = Fajr,
                                        Tulu = Tulu,
                                        Zuhr = Zuhr,
                                        Asr = Asr,
                                        Maghrib = Maghrib,
                                        Isha = Isha,
                                        CityId = cityDbResult.Id,
                                        Date = new DateTime(DateTime.Now.Year,month+1,day)
                                    });
                                    
                                    _logger.LogInformation("Namaz saati verisi ön listeye başarıyla eklendi.");
                                }
                            }
                        }
                    }
                }

                await _context.Prayers.AddRangeAsync(prayers, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            
            _logger.LogInformation($"Namaz vakitleri listesi başarıyla veritabanına eklendi.({DateTime.Now.Year})");
            return Result<long>.Success(1,"Namaz vakitleri başarıyla eklendi.");
        }
        
    }
}