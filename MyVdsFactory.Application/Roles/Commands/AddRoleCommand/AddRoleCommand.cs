using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Roles.Commands.AddRoleCommand;

public class AddRoleCommand : IRequest<Result<long>>
{
    public string? RoleName { get; set; }
    
    public class Handler : IRequestHandler<AddRoleCommand, Result<long>>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<AddRoleCommand> _logger;

        public Handler(RoleManager<Role> roleManager, ILogger<AddRoleCommand> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }
        
        public async Task<Result<long>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var isExist = await _roleManager.RoleExistsAsync(request.RoleName);
                
            if(!isExist && request.RoleName.IsNullOrEmpty().Equals(false))
            {
                await _roleManager.CreateAsync(new Role{
                    Name = request.RoleName
                });
                _logger.LogInformation("Rol ekleme girişimi başarılı oldu.");
            
                return Result<long>.Success(1,"Rol başarıyla eklendi.");
            }
            
            return Result<long>.Failure(new List<string>{"Rol ekleme işlemi başarısız oldu."});
        }
    }
}