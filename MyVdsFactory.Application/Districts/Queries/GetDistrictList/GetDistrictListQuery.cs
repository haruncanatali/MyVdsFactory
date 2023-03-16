using MediatR;

namespace MyVdsFactory.Application.Districts.Queries.GetDistrictList;

public class GetDistrictListQuery : IRequest<GetDistrictListVm>
{
    public string? CityName { get; set; }
    public long? CityId { get; set; }
}