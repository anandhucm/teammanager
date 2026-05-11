using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MYTEAMMANAGER.Models.Entities;

namespace MYTEAMMANAGER.Interfaces
{
    public interface IMemberRepository
    {
        void Update(TeamMember teamMember);
        
        Task<bool> SaveAllAsync();

        Task<IReadOnlyList<TeamMember>> GetMembersAsync();

        Task<TeamMember?> getMemberByIdAsync(string id);

        Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId);
    }
}