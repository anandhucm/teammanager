using MYTEAMMANAGER.Interfaces;
using MYTEAMMANAGER.Models;
using MYTEAMMANAGER.Models.Entities;

namespace MYTEAMMANAGER.Extensions
{

    // this class method is an extension method to the User. this keyword is used to define that. the method ToDto is defined outside the User class.
    public static class TeamMemberExtensions
    {
        public static MemberDto ToDto(this User user, ITokenService iTokenService)
        {

            var member = new MemberDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Token = iTokenService.CreateToken(user),
            };

            return member;

        }
    }
}