using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<Result<long>>
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long IdentityNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string>? RoleNames { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }

        public class Handler : IRequestHandler<CreateUserCommand, Result<long>>
        {
            private readonly UserManager<User> _userManager;
            private readonly RoleManager<Role> _roleManager;
            private readonly IApplicationContext _context;
            private readonly ILogger<CreateUserCommand> _logger;

            public Handler(UserManager<User> userManager, RoleManager<Role> roleManager,IApplicationContext context, ILogger<CreateUserCommand> logger)
            {
                _userManager = userManager;
                _roleManager = roleManager;
                _context = context;
                _logger = logger;
            }

            public async Task<Result<long>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    bool dublicateControl = _context.Users.Any(x => x.UserName == request.Username);
                    if (dublicateControl)
                    {
                        return Result<long>.Failure(new List<string>{"Aynı kullanıcı adına sahip kullanıcı mevcut"});
                    }

                    User entity = new()
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        UserName = request.Username,
                        Email = request.Email,
                        IdentityNumber = request.IdentityNumber,
                        Birthdate = request.BirthDate,
                        RefreshToken = String.Empty,
                        RefreshTokenExpiredTime = DateTime.Now,
                        PhoneNumber = request.Phone
                    };
                    
                    string password = new Random().Next(10000000, 99999999).ToString();
                    
                    if (!string.IsNullOrEmpty(request.Password))
                    {
                        password = request.Password;
                    }

                    await _userManager.CreateAsync(entity, password);
                    
                    _logger.LogInformation("Kullanıcı ekleme girişimi başarılı oldu.");


                    if (request.RoleNames is { Count: > 0 })
                    {
                        foreach (var roleName in request.RoleNames)
                        {
                            Role? role = await _roleManager.FindByNameAsync(roleName);
                            if (role != null)
                            {
                                await _userManager.AddToRoleAsync(entity, roleName);
                            }
                        }
                    }
                    else
                    {
                        setRole:
                        Role? role = await _roleManager.FindByNameAsync("Normal");
                        if (role != null)
                        {
                            await _userManager.AddToRoleAsync(entity, role.Name!);
                        }
                        else
                        {
                            await _roleManager.CreateAsync(new Role
                            {
                                Name = "Normal"
                            });
                            goto setRole;
                        }
                    }
                    
                    _logger.LogInformation("Kullanıcıya rol ekleme girişimi başarılı oldu.");

                    return Result<long>.Success(entity.Id,"Kullanıcı başarıyla oluşturuldu.");
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }