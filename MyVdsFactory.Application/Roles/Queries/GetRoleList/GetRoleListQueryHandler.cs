using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyVdsFactory.Application.Roles.Queries.Dtos;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Roles.Queries.GetRoleList;

public class GetRoleListQueryHandler : IRequestHandler<GetRoleListQuery,GetRoleListVm>
{
    private readonly IMapper _mapper;
    private readonly RoleManager<Role> _roleManager;

    public GetRoleListQueryHandler(IMapper mapper, RoleManager<Role> roleManager)
    {
        _mapper = mapper;
        _roleManager = roleManager;
    }

    public async Task<GetRoleListVm> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
    {
        var query = _roleManager.Roles;

        if (request.RoleName.IsNullOrEmpty().Equals(false))
        {
            query = query.Where(c => c.Name == request.RoleName);
        }

        var result = await query.ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new GetRoleListVm
        {
            Roles = result,
            Count = result?.Count
        };
    }
}