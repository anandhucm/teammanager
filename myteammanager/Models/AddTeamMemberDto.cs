
using System.ComponentModel.DataAnnotations;

namespace MYTEAMMANAGER.Models
{
    public class AddTeamMemberDto
    {
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; } = "";

        public string? MiddleName { get; set; }

        [Required]
        // [Required(AllowEmptyStrings = true)]  // this will only validates the null not the empty string.
        public required string LastName { get; set; }

        [Required]
        public required float Age { get; set; }

        public string? MobileNumber { get; set; }  //(question mark is to make this a nullable property)

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string EmployeeCode { get; set; }

        [Required]
        [MinLength(4)]
        public required string Password { get; set; }

        [Required]
        public required string UserName { get; set; }
    }
}