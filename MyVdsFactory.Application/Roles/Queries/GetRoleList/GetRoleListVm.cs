using MyVdsFactory.Application.Roles.Queries.Dtos;

namespace MyVdsFactory.Application.Roles.Queries.GetRoleList;

public class GetRoleListVm
{
    public List<RoleDto>? Roles { get; set; }
    public long? Count { get; set; }
}