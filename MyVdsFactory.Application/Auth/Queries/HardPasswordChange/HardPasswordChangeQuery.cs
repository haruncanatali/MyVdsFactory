using MediatR;
using Microsoft.AspNetCore.Identity;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Auth.Queries.HardPasswordChange;

public class HardPasswordChangeQuery : IRequest<Result<User>>
{
    public string Password { get; set; }
    public long UserId { get; set; }

    public class Handler : IRequestHandler<HardPasswordChangeQuery, Result<User>>
    {
        private readonly UserManager<User> _userManager;

        public Handler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<User>> Handle(HardPasswordChangeQuery request, CancellationToken cancellationToken)
        {
            User? appUser = _userManager.Users.FirstOrDefault(x => x.Id == request.UserId);
            if (appUser != null)
            {
                var removeResult = await _userManager.RemovePasswordAsync(appUser);
                if (removeResult.Succeeded)
                {
                    var addResult = await _userManager.AddPasswordAsync(appUser, request.Password);
                    if (addResult.Succeeded)
                    {
                        return Result<User>.Success(appUser);
                    }
                    else throw new Exception("Şifre Değiştirilemedi!");
                }
                else throw new Exception("Şifre Silinemedi!");
            }

            throw new Exception(nameof(User) + $"{request.UserId}");
        }
    }
}