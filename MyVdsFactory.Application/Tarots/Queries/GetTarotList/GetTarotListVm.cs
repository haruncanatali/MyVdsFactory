using MyVdsFactory.Application.Tarots.Queries.Dtos;

namespace MyVdsFactory.Application.Tarots.Queries.GetTarotList;

public class GetTarotListVm
{
    public List<TarotDto> Tarots { get; set; }
    public int Count { get; set; }
}