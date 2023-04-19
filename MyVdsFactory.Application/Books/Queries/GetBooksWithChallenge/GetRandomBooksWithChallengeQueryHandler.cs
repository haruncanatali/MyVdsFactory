using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyVdsFactory.Application.Books.Queries.Dtos;
using MyVdsFactory.Application.Books.Queries.GetRandomBook;
using MyVdsFactory.Application.Common.Interfaces;

namespace MyVdsFactory.Application.Books.Queries.GetBooksWithChallenge;

public class GetRandomBooksWithChallengeQueryHandler : IRequestHandler<GetRandomBooksWithChallengeQuery,GetRandomBooksWithChallengeVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetRandomBooksWithChallengeQueryHandler> _logger;

    public GetRandomBooksWithChallengeQueryHandler(IApplicationContext context, IMapper mapper, ILogger<GetRandomBooksWithChallengeQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetRandomBooksWithChallengeVm> Handle(GetRandomBooksWithChallengeQuery request, CancellationToken cancellationToken)
    {
        var bookDbSize = await _context.Books.CountAsync(cancellationToken);
        var authorDbSize = await _context.Authors.CountAsync(cancellationToken);
        
        Random random = new Random();
        var randomIndex = random.Next(0, bookDbSize);
        List<string> options = new List<string>();

        var randomBook = await _context.Books
            .Skip(randomIndex)
            .Take(1)
            .Include(c => c.Author)
            .ProjectTo<BookChallengeDto>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
        
        options.Add(randomBook.AuthorName);

        while (options.Count != 4)
        {
            x:
            var randomAuthorIndex = random.Next(0, authorDbSize);
            var randomAuthorName = await _context.Authors.Skip(randomAuthorIndex).Take(1)
                .Select(c => c.FullName).FirstAsync(cancellationToken);
            if (randomAuthorName == randomBook.AuthorName) goto x;
            options.Add(randomAuthorName);
        }
        
        await Shuffle(options);

        randomBook.Options = options;

        _logger.LogInformation($"Rastgele kitap - ID:{randomBook.BookId} - çekildi.");

        return new GetRandomBooksWithChallengeVm
        {
            BookChallengeModel = randomBook
        };
        
    }
    
    private async Task Shuffle<T>(List<T> list) 
    {
        Random rand = new Random();
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rand.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}