using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<Result<Unit>>
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long IdentityNumber { get; set; }
        public string Email { get; set; }
        public List<string>? RoleNames { get; set; }
        public string Phone { get; set; }
        public DateTime Birthdate { get; set; }

        public class Handler : IRequestHandler<UpdateUserCommand, Result<Unit>>
        {
            private readonly UserManager<User> _userManager;
            private readonly RoleManager<Role> _roleManager;
            private readonly IApplicationContext _context;

            public Handler(UserManager<User> userManager, RoleManager<Role> roleManager,
                IApplicationContext context)
            {
                _userManager = userManager;
                _roleManager = roleManager;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);
                    
                    if (user == null)
                    {
                        return Result<Unit>.Failure(new List<string>{"Güncellenecek kullanıcı bulunamadı."});
                    }

                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;
                    user.UserName = request.Username;
                    user.Email = request.Email;
                    user.IdentityNumber = request.IdentityNumber;
                    user.Birthdate = request.Birthdate;
                    user.PhoneNumber = request.Phone;
                    
                    if (request.RoleNames is {Count: > 0})
                    {
                        foreach (var roleName in request.RoleNames)
                        {
                            Role? role = await _roleManager.FindByNameAsync(roleName);
                            if (role != null)
                            {
                                var dublicateControl = await _userManager.IsInRoleAsync(user, roleName);
                                if (!dublicateControl)
                                {
                                    await _userManager.AddToRoleAsync(user, roleName);
                                }
                            }
                        }
                    }

                    var db_result = await _context.SaveChangesAsync(cancellationToken);

                    if (db_result == 0)
                    {
                        return Result<Unit>.Failure(new List<string>{"Kullanıcı güncellenemedi."});
                    }
                    
                    return Result<Unit>.Success(Unit.Value,"Kullanıcı başarıyla güncellendi.");
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }