using MyVdsFactory.Application.Books.Queries.Dtos;

namespace MyVdsFactory.Application.Books.Queries.GetBooksWithAuthor;

public class GetBooksWithAuthorVm
{
    public List<BookDto> Books { get; set; }
    public int Count { get; set; }
}