using Microsoft.EntityFrameworkCore;
using MYTEAMMANAGER.Models.Entities;

namespace MYTEAMMANAGER.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

        // here the DbSet type is the class name and variable is the name that we used in the code to acess the database of the corresponding class.
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Photo> Photos { get; set; }

    }
}