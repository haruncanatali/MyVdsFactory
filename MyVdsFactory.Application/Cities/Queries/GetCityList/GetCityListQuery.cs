using MediatR;

namespace MyVdsFactory.Application.Cities.Queries.GetCityList;

public class GetCityListQuery : IRequest<GetCityListVm>
{
    public string? Name { get; set; }
}