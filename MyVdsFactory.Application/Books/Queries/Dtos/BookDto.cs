using AutoMapper;
using MyVdsFactory.Application.Common.Mappings;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Books.Queries.Dtos;

public class BookDto : IMapFrom<Book>
{
    public long BookId { get; set; }
    public string BookName { get; set; }
    public string AuthorId { get; set; }
    public string AuthorName { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Book, BookDto>()
            .ForMember(dest => dest.BookId, opt =>
                opt.MapFrom(c => c.Id))
            .ForMember(dest => dest.BookName, opt =>
                opt.MapFrom(c => c.Name))
            .ForMember(dest => dest.AuthorId, opt =>
                opt.MapFrom(c => c.Author.Id))
            .ForMember(dest => dest.AuthorName, opt =>
                opt.MapFrom(c => c.Author.FullName));
    }
}