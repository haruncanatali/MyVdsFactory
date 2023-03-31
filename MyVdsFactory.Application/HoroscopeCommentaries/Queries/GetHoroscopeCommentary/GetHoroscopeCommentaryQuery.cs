using MediatR;

namespace MyVdsFactory.Application.HoroscopeCommentaries.Queries.GetHoroscopeCommentary;

public class GetHoroscopeCommentaryQuery : IRequest<GetHoroscopeCommentaryVm>
{
    public long Id { get; set; }
}