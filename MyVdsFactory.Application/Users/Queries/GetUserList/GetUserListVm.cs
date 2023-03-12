using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyVdsFactory.Application.Users.Queries.Dtos;

namespace MyVdsFactory.Application.Users.Queries.GetUserList
{
    public class GetUserListVm
    {
        public List<UserResponseDto> Users { get; set; }
        public long Count { get; set; }
    }
}
