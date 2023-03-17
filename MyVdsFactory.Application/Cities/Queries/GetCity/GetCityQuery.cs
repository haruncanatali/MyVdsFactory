using MediatR;

namespace MyVdsFactory.Application.Cities.Queries.GetCity;

public class GetCityQuery : IRequest<GetCityVm>
{
    public long Id { get; set; }
}