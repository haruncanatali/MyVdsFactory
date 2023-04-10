using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HtmlAgilityPack;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyVdsFactory.Application.Common.Extensions;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

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
                List<TarotSingleBase> tarotSingleBases = new List<TarotSingleBase>();
                List<Tarot> tarots = new List<Tarot>();
                
                var cloudName = await _context.Accounts
                    .FirstOrDefaultAsync(c => c.Platform == "Cloudinary" && c.ObjectTitle == "cloud_name",
                        cancellationToken);
                var apiKey = await _context.Accounts
                    .FirstOrDefaultAsync(c => c.Platform == "Cloudinary" && c.ObjectTitle == "api_key", 
                        cancellationToken);
                var apiSecret = await _context.Accounts
                    .FirstOrDefaultAsync(c => c.Platform == "Cloudinary" && c.ObjectTitle == "api_secret",
                        cancellationToken);
                
                var cloudinary = new Cloudinary(new CloudinaryDotNet.Account(cloudName!.ObjectValue, apiKey!.ObjectValue, apiSecret!.ObjectValue));

                var db_entity_count = await _context.Tarots.CountAsync(cancellationToken);

                if (db_entity_count > 0)
                {
                    await _context.Tarots.ExecuteDeleteAsync(cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                
                    _logger.LogInformation($"Veritabanındaki tüm tarot verisi silindi.)");
                }
                
                HtmlDocument pageDocument;
                var response = await client.GetAsync(
                    $"https://www.boxerdergisi.com.tr/tarot-kartlarinin-anlamlari");
                var pageContents = await response.Content.ReadAsStringAsync(cancellationToken);
                    
                _logger.LogInformation("Tarot verisi isimlendirmeleri için url e istek atıldı.");
                    
                pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContents);

                var tarotsMain =
                    pageDocument.DocumentNode?
                        .SelectSingleNode("/html[1]/body[1]/div[2]/div[2]/div[1]/div[1]");

                foreach (var tarotSingle in tarotsMain.ChildNodes)
                {
                    if (tarotSingle.InnerHtml.IsNullOrEmpty().Equals(true)
                        || string.IsNullOrWhiteSpace(tarotSingle.InnerHtml))
                    {
                        continue;
                    }

                    foreach (var items in tarotSingle.ChildNodes)
                    {
                        if (items.InnerHtml.IsNullOrEmpty().Equals(true)
                            || string.IsNullOrWhiteSpace(items.InnerHtml))
                        {
                            continue;
                        }

                        var photoInnerHtml = items.ChildNodes[1].InnerHtml;
                        var tarotNameInnerHtml = items.ChildNodes[3].InnerHtml.HtmlDecodeToString();
                        
                        var photoUrl = "https://www.boxerdergisi.com.tr"+photoInnerHtml.Split("\"")[1];
                        var tarotName = tarotNameInnerHtml.Trim();
                        var tarotNormalizedName =
                            (tarotName.Replace(" ", "-").Replace("(", "").Replace(")", ""))
                            .ReplaceTurkishCharacters();
                        var tarotSourceUrl =
                            "https://www.boxerdergisi.com.tr/tarot-karti-anlami/" + tarotNormalizedName;

                        tarotSingleBases.Add(new TarotSingleBase
                        {
                            PhotoUrl = photoUrl,
                            TarotName = tarotName,
                            TarotNormalizedName = tarotNormalizedName,
                            TarotSourceUrl = tarotSourceUrl
                        });
                        
                        _logger.LogInformation($"Tarot ana verisi çekildi.({tarotName})");
                    }
                    
                }

                foreach (var tarotSingleBase in tarotSingleBases)
                {
                    response = await client.GetAsync(tarotSingleBase.TarotSourceUrl);
                    pageContents = await response.Content.ReadAsStringAsync(cancellationToken);
                    
                    _logger.LogInformation
                        ($"Tarot verisi detayı için url e istek atıldı.({tarotSingleBase.TarotSourceUrl})");
                    
                    pageDocument = new HtmlDocument();
                    pageDocument.LoadHtml(pageContents);

                    int counter = 1;
                    string? tarotPrg = "";
                    
                    while (true)
                    {
                        counter++;

                        var content = "";
                        
                        try
                        { 
                            content = pageDocument.DocumentNode?
                                .SelectSingleNode($"/html[1]/body[1]/div[2]/div[2]/div[1]/div[1]/p[{counter}]").InnerText + "\n\n";
                        }
                        catch (Exception e)
                        {
                            Tarot tarot = new Tarot();

                            tarot.Name = tarotSingleBase.TarotName ?? "-";
                            tarot.Description = tarotPrg.HtmlDecodeToString();
                            tarot.Features = "Eklenmemiş";
                            

                            var uploadParams = new ImageUploadParams()
                            {
                                File = new FileDescription(@$"{tarotSingleBase.PhotoUrl}"),
                                PublicId = $"{tarotSingleBase.TarotNormalizedName}"
                            };

                            var uploadResult = cloudinary.Upload(uploadParams);
                            
                            _logger.LogInformation($"" +
                                                   $"Tarot resmi cloudinary hesabına eklendi." +
                                                   $"({tarotSingleBase.TarotNormalizedName})");

                            if ((bool)uploadResult.Url?.ToString().IsNullOrEmpty().Equals(false))
                            {
                                tarot.PhotoUrl = cloudinary.Api
                                    .UrlImgUp
                                    .BuildUrl(tarotSingleBase.TarotNormalizedName)
                                    .Insert(4, "s")
                                    .Replace("%2C",",")
                                    .Replace("%0","")
                                    .Replace("v1/","")
                                    .Replace("Aw_1000","w_1000");
                            }
                            
                            tarots.Add(tarot);
                            
                            break;
                        }

                        if (content.IsNullOrEmpty().Equals(false) 
                            && string.IsNullOrWhiteSpace(content).Equals(false)
                            && content.Contains("-").Equals(false))
                        {
                            tarotPrg += content
                                .Replace("&nbsp;", "")
                                .Replace("\n", "")
                                .Replace("\r", "");
                        }
                        else
                        {
                            var tarotFeatures = pageDocument.DocumentNode?
                                .SelectSingleNode($"/html[1]/body[1]/div[2]/div[2]/div[1]/div[1]/p[{counter}]")
                                .InnerText
                                .Replace("\r", "")
                                .Replace("&nbsp;","")
                                .Replace("\n","")
                                .Split('-');
                            
                            Tarot tarot = new Tarot();

                            tarot.Name = tarotSingleBase.TarotName ?? "-";
                            tarot.Description = tarotPrg.HtmlDecodeToString();
                            tarot.Features = string.Join('\n', tarotFeatures).HtmlDecodeToString();
                            

                            var uploadParams = new ImageUploadParams()
                            {
                                File = new FileDescription(@$"{tarotSingleBase.PhotoUrl}"),
                                PublicId = $"{tarotSingleBase.TarotNormalizedName}"
                            };

                            var uploadResult = cloudinary.Upload(uploadParams);
                            
                            _logger.LogInformation($"" +
                                                   $"Tarot resmi cloudinary hesabına eklendi." +
                                                   $"({tarotSingleBase.TarotNormalizedName})");

                            if ((bool)uploadResult.Url?.ToString().IsNullOrEmpty().Equals(false))
                            {
                                tarot.PhotoUrl = cloudinary.Api
                                    .UrlImgUp
                                    .BuildUrl(tarotSingleBase.TarotNormalizedName)
                                    .Insert(4, "s")
                                    .Replace("%2C",",")
                                    .Replace("%0","")
                                    .Replace("v1/","")
                                    .Replace("Aw_1000","w_1000");
                            }
                            
                            tarots.Add(tarot);
                            
                            break;
                        }
                    }

                }

                await _context.Tarots.AddRangeAsync(tarots, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                return Result<long>.Success(1,"Tarot verileri başarıyla eklendi.");
            }
        }
    }
    
    public struct TarotSingleBase
    {
        public string PhotoUrl { get; set; }
        public string TarotName { get; set; }
        public string TarotNormalizedName { get; set; }
        public string TarotSourceUrl { get; set; }
    }
}