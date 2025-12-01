
using System.ComponentModel.DataAnnotations;

namespace MYTEAMMANAGER.Models
{
    public class LoginTeamMemberDto
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(4)]
        public required string Password { get; set; }
        
        [Required] 
        public required string UserName { get; set; }
    }
}