using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Books.Queries.Dtos;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Books.Queries.GetBooksWithAuthor;

public class GetBooksWithAuthorQueryHandler : IRequestHandler<GetBooksWithAuthorQuery,GetBooksWithAuthorVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetBooksWithAuthorQueryHandler> _logger;

    public GetBooksWithAuthorQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetBooksWithAuthorQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetBooksWithAuthorVm> Handle(GetBooksWithAuthorQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Book> query = _context.Books.Include(c=>c.Author);

        if (request.AuthorName != null)
        {
            query = query.Where(c => c.Author.FullName == request.AuthorName);
        }

        var result = await query
            .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        var count = result.Count;
        
        _logger.LogInformation("Kitap listesi çekildi.");

        return new GetBooksWithAuthorVm
        {
            Books = result,
            Count = count
        };
    }
}