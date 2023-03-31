using MediatR;

namespace MyVdsFactory.Application.Accounts.Queries.GetAccount;

public class GetAccountQuery : IRequest<GetAccountVm>
{
    public long Id { get; set; }
}