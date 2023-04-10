using AutoMapper;
using MyVdsFactory.Application.Common.Mappings;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Tarots.Queries.Dtos;

public class TarotDto : IMapFrom<Tarot>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Features { get; set; }
    public string PhotoUrl { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Tarot, TarotDto>();
    }
}