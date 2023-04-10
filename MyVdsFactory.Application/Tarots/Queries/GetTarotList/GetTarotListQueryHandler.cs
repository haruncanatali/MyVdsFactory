using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Tarots.Queries.Dtos;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Tarots.Queries.GetTarotList;

public class GetTarotListQueryHandler : IRequestHandler<GetTarotListQuery,GetTarotListVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTarotListQueryHandler> _logger;

    public GetTarotListQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetTarotListQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetTarotListVm> Handle(GetTarotListQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Tarot> query = _context.Tarots;

        if (request.Name.IsNullOrEmpty().Equals(false))
        {
            query = query.Where(c => c.Name.Contains(request.Name!));
        }

        var result = await query.ProjectTo<TarotDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        return new GetTarotListVm
        {
            Tarots = result,
            Count = result.Count
        };
    }
}