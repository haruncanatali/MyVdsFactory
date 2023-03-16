using MyVdsFactory.Application.Districts.Queries.Dtos;

namespace MyVdsFactory.Application.Districts.Queries.GetDistrictList;

public class GetDistrictListVm
{
    public List<DistrictDto>? Districts { get; set; }
    public long Count { get; set; }
}