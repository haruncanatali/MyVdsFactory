using AutoMapper;
using MyVdsFactory.Application.Common.Mappings;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Accounts.Queries.Dtos;

public class AccountDto : IMapFrom<Account>
{
    public long Id { get; set; }
    public string Platform { get; set; }
    public string ObjectTitle { get; set; }
    public string ObjectValue { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Account, AccountDto>();
    }
}