namespace MyVdsFactory.Domain.Entities;

public class City : BaseEntity
{
    public City()
    {
        Districts = new List<District>();
    }
    
    public string Name { get; set; }

    public List<District> Districts { get; set; }
}