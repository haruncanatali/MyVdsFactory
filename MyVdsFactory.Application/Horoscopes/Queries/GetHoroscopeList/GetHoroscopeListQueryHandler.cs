using AutoMapper;
using AutoMapper.QueryableExtensions;
using CloudinaryDotNet;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Horoscopes.Queries.Dtos;
using MyVdsFactory.Domain.Entities;
using Account = CloudinaryDotNet.Account;

namespace MyVdsFactory.Application.Horoscopes.Queries.GetHoroscopeList;

public class GetHoroscopeListQueryHandler : IRequestHandler<GetHoroscopeListQuery,GetHoroscopeListVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetHoroscopeListQueryHandler> _logger;

    public GetHoroscopeListQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetHoroscopeListQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetHoroscopeListVm> Handle(GetHoroscopeListQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Horoscope> query = _context.Horoscopes;

        if (request.HoroscopeName.IsNullOrEmpty().Equals(false))
        {
            query = query.Where(c => c.Name.ToLower().Contains(request.HoroscopeName.ToLower()));
        }    
            
        var result = await query
            .OrderBy(c=>c.Id)
            .Include(c => c.HoroscopeCommentaries)
            .ProjectTo<HoroscopeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (result.Count > 0)
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
            
            result.ForEach(c =>
            {
                c.PhotoUrl = new Cloudinary(new Account(cloudName?.ObjectValue, apiKey?.ObjectValue, apiSecret?.ObjectValue))?.Api
                    .UrlImgUp
                    .BuildUrl(c.PhotoName)
                    .Insert(4, "s")
                    .Replace("%2C",",")
                    .Replace("%0","")
                    .Replace("v1/","")
                    .Replace("Aw_1000","w_1000");
            });
        }
        
        _logger.LogInformation("Burç verileri çekme girişiminde bulunuldu");

        return new GetHoroscopeListVm
        {
            Count = result.Count,
            Horoscopes = result
        };
    }
}