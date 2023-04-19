using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Books.Queries.Dtos;
using MyVdsFactory.Application.Common.Interfaces;

namespace MyVdsFactory.Application.Books.Queries.GetRandomBook;

public class GetRandomBookQueryHandler : IRequestHandler<GetRandomBookQuery,GetRandomBookVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetRandomBookQueryHandler> _logger;

    public GetRandomBookQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetRandomBookQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetRandomBookVm> Handle(GetRandomBookQuery request, CancellationToken cancellationToken)
    {
        var bookDbSize = await _context.Books.CountAsync(cancellationToken);
        Random random = new Random();
        var randomIndex = random.Next(0, bookDbSize);

        var randomBook = await _context.Books
            .Skip(randomIndex)
            .Take(1)
            .Include(c => c.Author)
            .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
        
        _logger.LogInformation($"Rastgele kitap - ID:{randomBook.BookId} - çekildi.");

        return new GetRandomBookVm
        {
            Book = randomBook
        };
    }
}