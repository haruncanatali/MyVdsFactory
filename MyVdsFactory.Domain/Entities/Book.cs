namespace MyVdsFactory.Domain.Entities;

public class Book : BaseEntity
{
    public string Name { get; set; }

    public long AurhorId { get; set; }

    public virtual Author Author { get; set; }
}