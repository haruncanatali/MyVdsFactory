using Microsoft.AspNetCore.Identity;

namespace MyVdsFactory.Domain.Entities;

public class Role : IdentityRole<long>
{
    public Role() : base()
    {

    }

    public Role(string roleName) : base(roleName)
    {

    }
}