using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.HoroscopeCommentaries.Queries.Dtos;

namespace MyVdsFactory.Application.HoroscopeCommentaries.Queries.GetHoroscopeCommentary;

public class GetHoroscopeCommentaryQueryHandler : IRequestHandler<GetHoroscopeCommentaryQuery,GetHoroscopeCommentaryVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetHoroscopeCommentaryQueryHandler> _logger;

    public GetHoroscopeCommentaryQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetHoroscopeCommentaryQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetHoroscopeCommentaryVm> Handle(GetHoroscopeCommentaryQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.HoroscopeCommentaries
            .ProjectTo<HoroscopeCommentaryDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(c => c.Id == request.Id,cancellationToken);

        _logger.LogInformation("Burç yorumları çekme girişimi");
        
        return new GetHoroscopeCommentaryVm
        {
            HoroscopeCommentary = result
        };
    }
}