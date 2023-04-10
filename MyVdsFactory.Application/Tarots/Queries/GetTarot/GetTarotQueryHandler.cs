using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Tarots.Queries.Dtos;

namespace MyVdsFactory.Application.Tarots.Queries.GetTarot;

public class GetTarotQueryHandler : IRequestHandler<GetTarotQuery,GetTarotVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTarotQueryHandler> _logger;

    public GetTarotQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetTarotQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetTarotVm> Handle(GetTarotQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Tarots
            .ProjectTo<TarotDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        
        _logger.LogInformation($"Tekil tarot verisi cekme girisimi(ID:{result?.Id})");

        return new GetTarotVm
        {
            Tarot = result
        };
    }
}