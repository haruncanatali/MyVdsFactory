using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;

namespace MyVdsFactory.Application.Accounts.Commands.UpdateAccountCommand;

public class UpdateAccountCommand : IRequest<Result<long>>
{
    public long Id { get; set; }
    public string Platform { get; set; }
    public string ObjectTitle { get; set; }
    public string ObjectValue { get; set; }
    
    public class Handler : IRequestHandler<UpdateAccountCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<UpdateAccountCommand> _logger;

        public Handler(IApplicationContext context, ILogger<UpdateAccountCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (account == null)
            {
                _logger.LogError("Hesap güncelleme girişimi başarısız oldu. ID="+request.Id);
                return Result<long>.Failure(new List<string> { "Güncellenecek hesap bulunamadı." });
            }

            account.Platform = request.Platform;
            account.ObjectTitle = request.ObjectTitle;
            account.ObjectValue = request.ObjectValue;

            _context.Accounts.Update(account);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Result<long>.Success(1,"Hesap güncelleme başarılı oldu.");
        }
    }
}