using MediatR;

namespace MyVdsFactory.Application.Cities.Queries.GetCityListWithoutDistricts;

public class GetCityListWithoutDistrictsQuery : IRequest<GetCityListWithoutDistrictsVm>
{
    public string? Name { get; set; }
}