using MediatR;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Accounts.Commands.AddAccountCommand;

public class AddAccountCommand : IRequest<Result<long>>
{
    public string Platform { get; set; }
    public string ObjectTitle { get; set; }
    public string ObjectValue { get; set; }
    
    public class Handler : IRequestHandler<AddAccountCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<AddAccountCommand> _logger;

        public Handler(IApplicationContext context, ILogger<AddAccountCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(AddAccountCommand request, CancellationToken cancellationToken)
        {
            var account = new Account
            {
                Platform = request.Platform,
                ObjectTitle = request.ObjectTitle,
                ObjectValue = request.ObjectValue
            };

            await _context.Accounts.AddAsync(account, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Hesap ekleme girişimi ID : "+account.Id);
            
            return Result<long>.Success(1,"Hesap başarıyla eklendi.");
        }
    }
}