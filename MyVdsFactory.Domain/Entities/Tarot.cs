namespace MyVdsFactory.Domain.Entities;

public class Tarot : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Features { get; set; }
    public string PhotoUrl { get; set; }
}