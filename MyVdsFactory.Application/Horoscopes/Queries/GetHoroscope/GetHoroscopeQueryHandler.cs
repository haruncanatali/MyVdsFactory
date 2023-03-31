using AutoMapper;
using AutoMapper.QueryableExtensions;
using CloudinaryDotNet;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Accounts.Queries.Dtos;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Horoscopes.Queries.Dtos;

namespace MyVdsFactory.Application.Horoscopes.Queries.GetHoroscope;

public class GetHoroscopeQueryHandler : IRequestHandler<GetHoroscopeQuery,GetHoroscopeVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetHoroscopeQueryHandler> _logger;

    public GetHoroscopeQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetHoroscopeQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetHoroscopeVm> Handle(GetHoroscopeQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Horoscopes
            .Include(c => c.HoroscopeCommentaries)
            .ProjectTo<HoroscopeDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(c=>c.Id == request.Id,cancellationToken);

        if (result != null)
        {
            var cloudName = await _context.Accounts
                .FirstOrDefaultAsync(c => c.Platform == "Cloudinary" && c.ObjectTitle == "cloud_name",
                    cancellationToken);
            var apiKey = await _context.Accounts
                .FirstOrDefaultAsync(c => c.Platform == "Cloudinary" && c.ObjectTitle == "api_key", 
                    cancellationToken);
            var apiSecret = await _context.Accounts
                .FirstOrDefaultAsync(c => c.Platform == "Cloudinary" && c.ObjectTitle == "api_secret",
                    cancellationToken);

            result.PhotoUrl = new Cloudinary(new Account(cloudName?.ObjectValue, apiKey?.ObjectValue, apiSecret?.ObjectValue))?.Api.UrlImgUp
                .Transform(new Transformation().Width(150).Height(150).Crop("fill")).BuildUrl(result.PhotoName);
        }
        
        _logger.LogInformation("Burç tekil verisi çekme girişimi : " 
                               + (result !=null ? $"ID={result.Id} bulundu." : $"ID={request.Id} bulunamadı."));

        return new GetHoroscopeVm
        {
            Horoscope = result
        };

    }
}