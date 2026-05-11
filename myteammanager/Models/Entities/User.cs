namespace MYTEAMMANAGER.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public required string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public required string LastName { get; set; }

        public required byte[] PasswordHash { get; set; }

        public required byte[] PasswordSalt { get; set; }

        public required string UserName { get; set; }

        public required string Email { get; set; }



    }
}