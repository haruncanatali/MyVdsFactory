namespace MyVdsFactory.Domain.Entities;

public class Author : BaseEntity
{
    public Author()
    {
        Books = new List<Book>();
    }
    
    public string FullName { get; set; }

    public List<Book> Books { get; set; }
}