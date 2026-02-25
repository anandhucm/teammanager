using MYTEAMMANAGER.Interfaces;
using MYTEAMMANAGER.Models;
using MYTEAMMANAGER.Models.Entities;

namespace MYTEAMMANAGER.Extensions
{
    public static class TeamMemberExtensions
    {
        public static MemberDto ToDto(this TeamMember teamMember, ITokenService iTokenService)
        {
            var member = new MemberDto()
            {
                Id = teamMember.Id,
                FirstName = teamMember.FirstName,
                LastName = teamMember.LastName,
                MiddleName = teamMember.MiddleName,
                EmployeeCode = teamMember.EmployeeCode,
                Token = iTokenService.CreateToken(teamMember)
            };

            return member;

        }
    }
}