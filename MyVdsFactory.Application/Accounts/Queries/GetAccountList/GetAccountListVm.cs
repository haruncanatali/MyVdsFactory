using MyVdsFactory.Application.Accounts.Queries.Dtos;

namespace MyVdsFactory.Application.Accounts.Queries.GetAccountList;

public class GetAccountListVm
{
    public List<AccountDto>? Accounts { get; set; }
    public long Count { get; set; }
}