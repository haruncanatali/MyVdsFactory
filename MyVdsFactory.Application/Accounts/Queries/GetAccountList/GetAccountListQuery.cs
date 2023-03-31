using MediatR;

namespace MyVdsFactory.Application.Accounts.Queries.GetAccountList;

public class GetAccountListQuery : IRequest<GetAccountListVm>
{
    public string? Platform { get; set; }
}