using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyVdsFactory.Application.Accounts.Queries.Dtos;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Accounts.Queries.GetAccountList;

public class GetAccountListQueryHandler : IRequestHandler<GetAccountListQuery,GetAccountListVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAccountListQueryHandler> _logger;

    public GetAccountListQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetAccountListQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetAccountListVm> Handle(GetAccountListQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Account> query = _context.Accounts as IQueryable<Account>;

        if (request.Platform.IsNullOrEmpty().Equals(false))
        {
            query = query.Where(c => c.Platform.ToLower() == request.Platform.ToLower());
        }

        var result = await query.ProjectTo<AccountDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        return new GetAccountListVm
        {
            Accounts = result,
            Count = result.Count
        };
    }
}