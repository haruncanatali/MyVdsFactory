namespace MyVdsFactory.Domain.Entities;

public class District : BaseEntity
{
    public string Name { get; set; }

    public long CityId { get; set; }

    public virtual City City { get; set; }
}