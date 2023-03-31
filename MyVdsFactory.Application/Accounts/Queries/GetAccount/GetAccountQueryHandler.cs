using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Accounts.Queries.Dtos;
using MyVdsFactory.Application.Common.Interfaces;

namespace MyVdsFactory.Application.Accounts.Queries.GetAccount;

public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery,GetAccountVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAccountQueryHandler> _logger;

    public GetAccountQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetAccountQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetAccountVm> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Accounts
            .ProjectTo<AccountDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        _logger.LogInformation("Hesap verisi çekme girişimi! ID="+result?.Id);

        return new GetAccountVm
        {
            Account = result
        };
    }
}