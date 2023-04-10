using MediatR;

namespace MyVdsFactory.Application.Tarots.Queries.GetRandomTarotList;

public class GetRandomTarotListQuery : IRequest<GetRandomTarotListVm>
{
    public int? Amount { get; set; }
}