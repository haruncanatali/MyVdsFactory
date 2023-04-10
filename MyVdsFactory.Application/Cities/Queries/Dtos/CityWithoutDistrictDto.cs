using AutoMapper;
using MyVdsFactory.Application.Common.Mappings;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Cities.Queries.Dtos;

public class CityWithoutDistrictDto : IMapFrom<City>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<City, CityWithoutDistrictDto>();
    }
}