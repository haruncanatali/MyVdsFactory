using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MyVdsFactory.Application.Users.Queries.GetUser
{
    public class GetUserQuery : IRequest<GetUserVm>
    {
        public long Id { get; set; }
    }
}
