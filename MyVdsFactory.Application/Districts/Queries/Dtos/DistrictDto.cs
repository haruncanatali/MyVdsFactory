using AutoMapper;
using MyVdsFactory.Application.Common.Mappings;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Districts.Queries.Dtos;

public class DistrictDto : IMapFrom<District>
{
    public string DistrictName { get; set; }
    public string CityName { get; set; }
    public long DistrictId { get; set; }
    public long CityId { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<District, DistrictDto>()
            .ForMember(dest => dest.DistrictName, opt =>
                opt.MapFrom(c => c.Name))
            .ForMember(dest => dest.DistrictId, opt =>
                opt.MapFrom(c => c.Id))
            .ForMember(dest => dest.CityName, opt =>
                opt.MapFrom(c => c.City.Name))
            .ForMember(dest => dest.CityId, opt =>
                opt.MapFrom(c => c.CityId));
    }
}