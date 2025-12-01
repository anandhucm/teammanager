using System;
using MYTEAMMANAGER.Models.Entities;

namespace MYTEAMMANAGER.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(TeamMember teamMember);
        
    }  
}