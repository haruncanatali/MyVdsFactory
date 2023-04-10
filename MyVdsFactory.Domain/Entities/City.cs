namespace MyVdsFactory.Domain.Entities;

public class City : BaseEntity
{
    public City()
    {
        Districts = new List<District>();
        Prayers = new List<Prayer>();
    }
    
    public string Name { get; set; }

    public List<District> Districts { get; set; }
    public List<Prayer> Prayers { get; set; }
}