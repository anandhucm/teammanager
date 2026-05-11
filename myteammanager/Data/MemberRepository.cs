using System;
using Microsoft.EntityFrameworkCore;
using MYTEAMMANAGER.Interfaces;
using MYTEAMMANAGER.Models.Entities;

namespace MYTEAMMANAGER.Data;

public class MemberRepository(ApplicationDbContext context) : IMemberRepository
{
    public async Task<TeamMember?> getMemberByIdAsync(string id)
    {
        if(!Guid.TryParse(id, out Guid guidId))
        {
            throw new Exception("Invalid guid format");                   
        }
        return await context.TeamMembers.FindAsync(guidId);
    }

    public async Task<IReadOnlyList<TeamMember>> GetMembersAsync()
    {
        // return await context.TeamMembers.Include(x=>x.User).ToListAsync();
        return await context.TeamMembers
        .Select(x => new TeamMember{
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            EmployeeCode = x.EmployeeCode,
            Email = x.Email,
            DateOfBirth = x.DateOfBirth,
            Age = x.Age,
        })
        .ToListAsync();
        
    }

    public Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(TeamMember teamMember)
    {
        context.Entry(teamMember).State = EntityState.Modified;
    }
}

