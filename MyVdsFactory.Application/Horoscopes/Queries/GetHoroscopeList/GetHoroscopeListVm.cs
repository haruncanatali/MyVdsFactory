using MyVdsFactory.Application.Horoscopes.Queries.Dtos;

namespace MyVdsFactory.Application.Horoscopes.Queries.GetHoroscopeList;

public class GetHoroscopeListVm
{
    public List<HoroscopeDto>? Horoscopes { get; set; }
    public long Count { get; set; }
}