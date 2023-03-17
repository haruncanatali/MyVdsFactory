using MyVdsFactory.Application.Cities.Queries.Dtos;

namespace MyVdsFactory.Application.Cities.Queries.GetCityList;

public class GetCityListVm
{
    public List<CityDto>? Cities { get; set; }
    public long Count { get; set; }
}