using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityFeature.Application.DTO;
using IdentityFeature.Application.Repository;
using MediatR;

namespace IdentityFeature.Application.Queries
{
    public class GetTeamMembersQueryHandler : IRequestHandler<GetTeamMembersQuery, IEnumerable<TeamMemberDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public GetTeamMembersQueryHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<TeamMemberDto>> Handle(GetTeamMembersQuery request, CancellationToken cancellationToken)
        {
            var usersResult = await _userRepository.GetItemsWithCountAsync<string>(x => true, new HRMS.Core.Postgres.Common.Request { PageCriteria = new HRMS.Core.Postgres.Common.PageCriteria { PageSize = 1000, Skip = 0 } });
            var users = usersResult.data;

            var rolesResult = await _roleRepository.GetItemsWithCountAsync<string>(x => true, new HRMS.Core.Postgres.Common.Request { PageCriteria = new HRMS.Core.Postgres.Common.PageCriteria { PageSize = 1000, Skip = 0 } });
            var roles = rolesResult.data;
            
            var result = new List<TeamMemberDto>();
            foreach(var user in users)
            {
                var role = roles.FirstOrDefault(r => r.Id == user.RoleId);
                result.Add(new TeamMemberDto
                {
                    Id = user.Id,
                    Name = $"{user.FirstName} {user.LastName}".Trim(),
                    Email = user.Email,
                    Designation = role != null ? role.Name : "Employee",
                    Department = "Engineering", // Default mocked dept since it's not in User entity
                    Status = "active",
                    Manager = "Michael Chen",
                    ReportingManager = "Rachel Green",
                    JoinDate = "2026-01-01",
                    Location = "Office",
                    Avatar = user.FirstName.Substring(0, 1) + user.LastName.Substring(0, 1),
                    Color = "bg-teal-600"
                });
            }
            
            return result;
        }
    }
}
