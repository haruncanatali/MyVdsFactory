using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Users.Queries.Dtos;

namespace MyVdsFactory.Application.Users.Queries.GetUserList
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery,GetUserListVm>
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;

        public GetUserListQueryHandler(IApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetUserListVm> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var userListQuery = _context.Users.ProjectTo<UserResponseDto>(_mapper.ConfigurationProvider);

            if (userListQuery == null)
            {
                return null;
            }

            if (!request.FirstName.IsNullOrEmpty())
            {
                userListQuery = userListQuery.Where(c => c.FirstName.ToLower() == request.FirstName.ToLower());
            }

            if (!request.LastName.IsNullOrEmpty())
            {
                userListQuery = userListQuery.Where(c => c.LastName.ToLower() == request.LastName.ToLower());
            }

            var userRoles = await _context.UserRoles.Where(c => (userListQuery.Select(c => c.Id).Contains(c.UserId)))
                .ToListAsync(cancellationToken);


            var userList = await userListQuery.ToListAsync(cancellationToken: cancellationToken);

            foreach (var user in userList)
            {
                var userRolesId = userRoles.Where(c => c.UserId == user.Id).Select(c => c.RoleId).ToList();
                user.Roles = await _context.Roles.Where(c => userRolesId.Contains(c.Id)).Select(c => c.Name)
                    .ToListAsync(cancellationToken);
            }

            var count = userList.Count;

            return new GetUserListVm
            {
                Users = userList,
                Count = count
            };
        }
    }
}
