using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Extensions;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Tarots.Queries.Dtos;

namespace MyVdsFactory.Application.Tarots.Queries.GetRandomTarotList;

public class GetRandomTarotListQueryHandler : IRequestHandler<GetRandomTarotListQuery,GetRandomTarotListVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetRandomTarotListQueryHandler> _logger;

    public GetRandomTarotListQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetRandomTarotListQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetRandomTarotListVm> Handle(GetRandomTarotListQuery request, CancellationToken cancellationToken)
    {
        var ids = (await _context.Tarots.Select(c => c.Id).ToListAsync(cancellationToken));
        ids.Shuffle(new Random());

        List<long> selectedIds = ids.Take(request.Amount ?? 4).ToList();
        
        _logger.LogInformation($"Rastgele tarot kartlari olusturuldu.({request.Amount ?? 4} tane)");

        var result = await _context.Tarots
            .Where(c => selectedIds.Contains(c.Id))
            .OrderBy(c => c.Id)
            .ProjectTo<TarotDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        _logger.LogInformation($"Rastgele tarot kartlari gonderildi.({request.Amount ?? 4} tane)");

        return new GetRandomTarotListVm
        {
            Tarots = result,
            Count = result.Count
        };
    }
}