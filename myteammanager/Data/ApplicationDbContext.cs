using Microsoft.EntityFrameworkCore;
using MYTEAMMANAGER.Models.Entities;

namespace MYTEAMMANAGER.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        
        public DbSet<TeamMember> TeamMembers { get; set; }

    }
}