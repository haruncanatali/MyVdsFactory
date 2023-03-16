using MediatR;

namespace MyVdsFactory.Application.Districts.Queries.GetDistrict;

public class GetDistrictQuery : IRequest<GetDistrictVm>
{
    public long Id { get; set; }
}