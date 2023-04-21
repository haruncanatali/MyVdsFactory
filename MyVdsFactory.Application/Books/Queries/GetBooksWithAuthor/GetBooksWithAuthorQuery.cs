using MediatR;

namespace MyVdsFactory.Application.Books.Queries.GetBooksWithAuthor;

public class GetBooksWithAuthorQuery : IRequest<GetBooksWithAuthorVm>
{
    public string? AuthorName { get; set; }
}