using MediatR;

namespace MyVdsFactory.Application.Horoscopes.Queries.GetHoroscopeList;

public class GetHoroscopeListQuery : IRequest<GetHoroscopeListVm>
{
    public string? HoroscopeName { get; set; }
}