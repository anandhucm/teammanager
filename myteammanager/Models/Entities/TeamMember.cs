namespace MYTEAMMANAGER.Models.Entities
{
    public class TeamMember
    {
        public Guid Id { get; set; }

        public required string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public required string LastName { get; set; }

        public required float Age { get; set; }

        public string? MobileNumber { get; set; }  //(question mark is to make this a nullable property)

        public required string Email { get; set; }

        public required string EmployeeCode { get; set; }

        public required byte[] PasswordHash { get; set; }

        public required byte[] PasswordSalt { get; set; }

        public required string UserName { get; set; }



    }
}