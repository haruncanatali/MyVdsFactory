using MyVdsFactory.Application.Tarots.Queries.Dtos;

namespace MyVdsFactory.Application.Tarots.Queries.GetRandomTarotList;

public class GetRandomTarotListVm
{
    public List<TarotDto>? Tarots { get; set; }
    public long Count { get; set; }
}