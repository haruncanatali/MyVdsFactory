using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MyVdsFactory.Application.Users.Queries.GetUserList
{
    public class GetUserListQuery : IRequest<GetUserListVm>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
