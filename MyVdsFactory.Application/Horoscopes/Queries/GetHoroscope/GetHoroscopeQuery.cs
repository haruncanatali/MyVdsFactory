using MediatR;

namespace MyVdsFactory.Application.Horoscopes.Queries.GetHoroscope;

public class GetHoroscopeQuery : IRequest<GetHoroscopeVm>
{
    public long Id { get; set; }
}