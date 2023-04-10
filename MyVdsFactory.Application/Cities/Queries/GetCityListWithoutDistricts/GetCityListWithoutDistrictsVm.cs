using MyVdsFactory.Application.Cities.Queries.Dtos;

namespace MyVdsFactory.Application.Cities.Queries.GetCityListWithoutDistricts;

public class GetCityListWithoutDistrictsVm
{
    public List<CityWithoutDistrictDto> Cities { get; set; }
    public int Count { get; set; }
}