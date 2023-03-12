using Microsoft.AspNetCore.Identity;
using MyVdsFactory.Domain.Enums;

namespace MyVdsFactory.Domain.Entities;

public class Role : IdentityRole<long>
{
    public Role() : base()
    {
    }

    public Role(string roleName) : base(roleName)
    {

    }

    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long CreatedBy { get; set; }
    public long? DeletedBy { get; set; }
    public long? UpdatedBy { get; set; }

    public EntityStatus Status { get; set; } = EntityStatus.Active;
}