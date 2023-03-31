using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Application.Accounts.Commands.DeleteAccountCommand;

public class DeleteAccountCommand : IRequest<Result<long>>
{
    public long Id { get; set; }
    public class Handler : IRequestHandler<DeleteAccountCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<DeleteAccountCommand> _logger;

        public Handler(IApplicationContext context, ILogger<DeleteAccountCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(c => c.Id == request.Id,cancellationToken);

            if (account == null)
            {
                _logger.LogError("Hesap silme girişimi başarısız oldu. ID="+request.Id);
                return Result<long>.Failure(new List<string>{"Silinecek hesap bulunamadı."});
            }

            account.Status = EntityStatus.Hidden;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Hesap silme girişimi başarılı oldu. Id="+request.Id);
            
            return Result<long>.Success(1,"Hesap silme başarılı oldu.");
        }
    }
}