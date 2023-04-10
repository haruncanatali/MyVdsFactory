using MediatR;

namespace MyVdsFactory.Application.Tarots.Queries.GetTarot;

public class GetTarotQuery : IRequest<GetTarotVm>
{
    public long Id { get; set; }
}