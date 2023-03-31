using MyVdsFactory.Application.HoroscopeCommentaries.Queries.Dtos;

namespace MyVdsFactory.Application.HoroscopeCommentaries.Queries.GetHoroscopeCommentaryList;

public class GetHoroscopeCommentaryListVm
{
    public List<HoroscopeCommentaryDto> HoroscopeCommentaries { get; set; }
    public long Count { get; set; }
}