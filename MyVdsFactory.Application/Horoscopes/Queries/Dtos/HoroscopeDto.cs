using AutoMapper;
using MyVdsFactory.Application.Common.Mappings;
using MyVdsFactory.Application.HoroscopeCommentaries.Queries.Dtos;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Horoscopes.Queries.Dtos;

public class HoroscopeDto : IMapFrom<Horoscope>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string PhotoName { get; set; }
    public string DateRange { get; set; }
    public string? PhotoUrl { get; set; }
    public string NormalizedName { get; set; }
    public string Description { get; set; }
    public string Planet { get; set; }
    public string Group { get; set; }
    
    public HoroscopeCommentaryDto? HoroscopeCommentary { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Horoscope, HoroscopeDto>()
            .ForMember(dest => dest.HoroscopeCommentary,
                opt => 
                opt.MapFrom(c=>c.HoroscopeCommentaries.FirstOrDefault(c=>c.Date.Date == DateTime.Now.Date)));
    }
}