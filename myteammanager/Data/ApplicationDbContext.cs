using Microsoft.EntityFrameworkCore;
using MYTEAMMANAGER.Models.Entities;

namespace MYTEAMMANAGER.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<TeamMember> TeamMembers { get; set; }

    }
}