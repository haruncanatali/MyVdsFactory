using MediatR;

namespace MyVdsFactory.Application.Tarots.Queries.GetTarotList;

public class GetTarotListQuery : IRequest<GetTarotListVm>
{
    public string? Name { get; set; }
}