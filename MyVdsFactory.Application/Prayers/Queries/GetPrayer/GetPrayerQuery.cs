using MediatR;

namespace MyVdsFactory.Application.Prayers.Queries.GetPrayer;

public class GetPrayerQuery : IRequest<GetPrayerVm>
{
    public long CityId { get; set; }
    public DateTime Date { get; set; }
}