using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Users.Queries.Dtos;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Users.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery,GetUserVm>
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetUserVm> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Users.ProjectTo<UserResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken: cancellationToken);

            if (result == null)
            {
                return null;
            }

            var userRoles = await _context.UserRoles.Where(c => c.UserId == result.Id).Select(c=>c.RoleId).ToListAsync(cancellationToken);
            var roles = await _context.Roles.Where(c => userRoles.Contains(c.Id)).Select(c => c.Name)
                .ToListAsync(cancellationToken);

            result.Roles = roles;
            
            return new GetUserVm
            {
                User = result
            };
        }
    }
}
