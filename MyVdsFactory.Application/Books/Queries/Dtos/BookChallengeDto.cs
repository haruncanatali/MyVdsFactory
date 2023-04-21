using AutoMapper;
using MyVdsFactory.Application.Common.Mappings;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Books.Queries.Dtos;

public class BookChallengeDto : IMapFrom<Book>
{
    public string BookName { get; set; }
    public string AuthorName { get; set; }
    public long BookId { get; set; }
    public long AuthorId { get; set; }
    public List<string> Options { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Book, BookChallengeDto>()
            .ForMember(dest => dest.BookName, opt =>
                opt.MapFrom(c => c.Name))
            .ForMember(dest => dest.BookId, opt =>
                opt.MapFrom(c => c.Id))
            .ForMember(dest => dest.AuthorId, opt =>
                opt.MapFrom(c => c.Author.Id))
            .ForMember(dest => dest.AuthorName, opt =>
                opt.MapFrom(c => c.Author.FullName));
    }
}