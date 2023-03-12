using AutoMapper;
using MyVdsFactory.Application.Common.Mappings;
using MyVdsFactory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVdsFactory.Application.Users.Queries.Dtos
{
    public class UserResponseDto : IMapFrom<User>
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long IdentityNumber { get; set; }
        public DateTime Birthdate { get; set; }
        public List<string> Roles { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserResponseDto>();
        }
    }
}
