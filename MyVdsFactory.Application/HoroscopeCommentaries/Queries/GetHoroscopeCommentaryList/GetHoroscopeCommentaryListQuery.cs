using MediatR;

namespace MyVdsFactory.Application.HoroscopeCommentaries.Queries.GetHoroscopeCommentaryList;

public class GetHoroscopeCommentaryListQuery : IRequest<GetHoroscopeCommentaryListVm>
{
    public long HoroscopeId { get; set; }
}