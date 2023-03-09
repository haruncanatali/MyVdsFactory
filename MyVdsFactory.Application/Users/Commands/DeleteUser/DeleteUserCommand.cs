using MediatR;
using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Result<Unit>>
{
    public long Id { get; set; }

    public class Handler : IRequestHandler<DeleteUserCommand, Result<Unit>>
    {
        private readonly IApplicationContext _context;

        public Handler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);
                    
                if (user == null)
                {
                    return Result<Unit>.Failure(new List<string>{"Silinecek kullanıcı bulunamadı."});
                }

                user.Status = EntityStatus.Passive;

                var db_result = await _context.SaveChangesAsync(cancellationToken);

                if (db_result == 0)
                {
                    return Result<Unit>.Failure(new List<string>{"Kullanıcı silinemedi."});
                }
                    
                return Result<Unit>.Success(Unit.Value,"Kullanıcı başarıyla silindi.");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}