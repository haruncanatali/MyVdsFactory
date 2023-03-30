using MediatR;
using Microsoft.AspNetCore.Identity;
using MyVdsFactory.Application.Auth.Dtos;
using MyVdsFactory.Application.Common.Managers;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Auth.Queries.RefreshToken;

public class RefreshTokenCommand: IRequest<Result<LoginDto>>
{
    public string RefreshToken { get; set; }

    public class Handler : IRequestHandler<RefreshTokenCommand, Result<LoginDto>>
    {
        private readonly TokenManager _tokenManager;
        private readonly UserManager<User> _userManager;

        public Handler(TokenManager tokenManager, UserManager<User> userManager)
        {
            _tokenManager = tokenManager;
            _userManager = userManager;
        }

        public async Task<Result<LoginDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            User? appUser = _userManager.Users.FirstOrDefault(x => x.RefreshToken == request.RefreshToken && x.RefreshTokenExpiredTime > DateTime.Now);
            if (appUser != null)
            {
                LoginDto loginViewModel = await _tokenManager.GenerateToken(appUser);
                return Result<LoginDto>.Success(data: loginViewModel);
            }

            return Result<LoginDto>.Failure(new List<string>() { "Başarısız." });
        }
    }
}