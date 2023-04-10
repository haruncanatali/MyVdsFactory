using MediatR;

namespace MyVdsFactory.Application.Prayers.Queries.GetPrayerList;

public class GetPrayerListQuery : IRequest<GetPrayerListVm>
{
    public long CityId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}