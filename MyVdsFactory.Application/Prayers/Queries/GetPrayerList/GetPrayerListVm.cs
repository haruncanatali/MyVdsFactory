using MyVdsFactory.Application.Prayers.Queries.Dtos;

namespace MyVdsFactory.Application.Prayers.Queries.GetPrayerList;

public class GetPrayerListVm
{
    public List<PrayerDto>? Prayers { get; set; }
    public long Count { get; set; }
}