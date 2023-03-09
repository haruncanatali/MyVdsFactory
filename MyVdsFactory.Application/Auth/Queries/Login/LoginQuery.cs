using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyVdsFactory.Application.Auth.Dtos;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Managers;
using MyVdsFactory.Application.Common.Mappings;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Auth.Queries.Login;

public class LoginQuery : IRequest<Result<LoginDto>>, IMapFrom<User>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public class Handler : IRequestHandler<LoginQuery, Result<LoginDto>>
        {
            private readonly SignInManager<User> _signInManager;

            private readonly TokenManager _tokenManager;
            private readonly UserManager<User> _userManager;
            private readonly IApplicationContext _context;
            private readonly IMapper _mapper;

            public Handler(SignInManager<User> signInManager, TokenManager tokenManager, UserManager<User> userManager,
                IApplicationContext context, IMapper mapper)
            {
                _signInManager = signInManager;
                _tokenManager = tokenManager;
                _userManager = userManager;
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<LoginDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
            {
                LoginDto loginViewModel = new LoginDto();
                User? appUser = await _userManager.FindByNameAsync(request.UserName);
                if (appUser != null)
                {
                    var result =
                        await _signInManager.PasswordSignInAsync(appUser.UserName, request.Password, false, false);
                    if (result.Succeeded)
                    {
                        loginViewModel = await _tokenManager.GenerateToken(appUser);
                        appUser.RefreshToken = loginViewModel.RefreshToken;
                        appUser.RefreshTokenExpiredTime = loginViewModel.RefreshTokenExpireTime;
                        await _userManager.UpdateAsync(appUser);
                        return Result<LoginDto>.Success(data: loginViewModel);
                    }
                }

                return Result<LoginDto>.Failure(new List<string>() { "Başarısız." });
            }
        }
    }