namespace MYTEAMMANAGER.Models
{
    public class AddTeamMemberDto
    {
        public Guid Id { get; set; }

        public required string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public required string LastName { get; set; }

        public required float Age { get; set; }

        public string? MobileNumber { get; set; }  //(question mark is to make this a nullable property)

        public required string Email { get; set; }

        public required string EmployeeCode { get; set; }
    }
}