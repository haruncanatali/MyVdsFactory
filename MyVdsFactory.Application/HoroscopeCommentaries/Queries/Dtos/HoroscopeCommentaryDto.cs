using AutoMapper;
using MyVdsFactory.Application.Common.Mappings;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.HoroscopeCommentaries.Queries.Dtos;

public class HoroscopeCommentaryDto : IMapFrom<HoroscopeCommentary>
{
    public long Id { get; set; }
    public long HoroscopeId { get; set; }
    public string Commentary { get; set; }
    public DateTime Date { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<HoroscopeCommentary, HoroscopeCommentaryDto>();
    }
}