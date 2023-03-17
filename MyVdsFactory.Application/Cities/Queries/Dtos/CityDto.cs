using AutoMapper;
using MyVdsFactory.Application.Common.Mappings;
using MyVdsFactory.Application.Districts.Queries.Dtos;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Cities.Queries.Dtos;

public class CityDto : IMapFrom<City>
{
    public long Id { get; set; }
    public string CityName { get; set; }
    public List<DistrictDto> Districts { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<City, CityDto>()
            .ForMember(dest => dest.CityName, opt =>
                opt.MapFrom(c => c.Name))
            .ForMember(dest => dest.Districts, opt =>
                opt.MapFrom(c => c.Districts));
    }
}