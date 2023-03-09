using Microsoft.AspNetCore.Identity;

namespace MyVdsFactory.Domain.Entities;

public class UserRole : IdentityUserRole<long>
{
    public UserRole() : base()
    {
            
    }
}