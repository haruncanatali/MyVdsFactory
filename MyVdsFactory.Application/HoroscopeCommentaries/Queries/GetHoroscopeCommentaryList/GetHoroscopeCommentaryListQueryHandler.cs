using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.HoroscopeCommentaries.Queries.Dtos;

namespace MyVdsFactory.Application.HoroscopeCommentaries.Queries.GetHoroscopeCommentaryList;

public class GetHoroscopeCommentaryListQueryHandler : IRequestHandler<GetHoroscopeCommentaryListQuery,GetHoroscopeCommentaryListVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetHoroscopeCommentaryListQueryHandler> _logger;

    public GetHoroscopeCommentaryListQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetHoroscopeCommentaryListQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetHoroscopeCommentaryListVm> Handle(GetHoroscopeCommentaryListQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.HoroscopeCommentaries
            .Where(c=>c.HoroscopeId == request.HoroscopeId)
            .ProjectTo<HoroscopeCommentaryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Burç yorumları çekme girişimi");
        
        return new GetHoroscopeCommentaryListVm
        {
            HoroscopeCommentaries = result,
            Count = result.Count
        };
    }
}