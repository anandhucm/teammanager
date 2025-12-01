
using System.ComponentModel.DataAnnotations;

namespace MYTEAMMANAGER.Models
{
    public class MemberDto
    {
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; } = "";

        public string? MiddleName { get; set; }

        [Required] 
        // [Required(AllowEmptyStrings = true)]  // this will only validates the null not the empty string.
        public required string LastName { get; set; }
        
        [Required]
        public required string EmployeeCode { get; set; }

        public string? ImageUrl { get; set; }

        public string Token { get; set; }
    }
}