using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MYTEAMMANAGER.Data;
using MYTEAMMANAGER.Models.Entities;
using System.Security.Cryptography;
using System.Text;

namespace MYTEAMMANAGER.Data;

public class Seed()
{
    public static async Task SeedUsers(ApplicationDbContext context)
    {
        if(await context.Users.AnyAsync()) return;
        
        var membersFromFile = await File.ReadAllTextAsync("/Users/anandhucm/Desktop/TEAM-MANAGER/myteammanager/Data/seed-user.json");
        var members = JsonSerializer.Deserialize<List<SeedUserDto>>(membersFromFile); // list of TeamMember Objects are created.

        if(members != null && members.Count > 0)
        {
            foreach (var member in members)
            {

                var hmac = new HMACSHA512();
                var user = new User
                {                
                    FirstName = member.FirstName,
                    MiddleName = member.MiddleName,
                    LastName = member.LastName,
                    UserName = member.UserName,
                    Email = member.Email,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(member.FirstName)),
                    PasswordSalt = hmac.Key
                };

                var teamMember = new TeamMember
                {                
                    FirstName = member.FirstName,
                    MiddleName = member.MiddleName,
                    LastName = member.LastName,
                    Email = member.Email,
                    Age = member.Age,
                    MobileNumber = member.MobileNumber,
                    EmployeeCode = member.EmployeeCode,
                    DateOfBirth = member.DateOfBirth,
                    User = user
                };

                var photo = new Photo
                {
                    Url = member.Url,
                    TeamMember = teamMember,

                };

                await context.AddRangeAsync(user, teamMember, photo);


                
         
            }

            await context.SaveChangesAsync();

          
        }
        
    }



}
