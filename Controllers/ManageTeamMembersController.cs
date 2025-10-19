using Microsoft.AspNetCore.Mvc;
using MYTEAMMANAGER.Data;
using MYTEAMMANAGER.Models.Entities;
using MYTEAMMANAGER.Models;
using Microsoft.EntityFrameworkCore;

namespace MYTEAMMANAGER.Controllers
{
    //localhost:xxxx/api/manageteammembers
    [Route("api/[controller]")]
    [ApiController]

    public class ManageTeamMembersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public ManageTeamMembersController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeamMembers()
        {
            var allEmployeeTeamMembers = await dbContext.TeamMembers.ToListAsync();
            return Ok(allEmployeeTeamMembers);

        }

        [HttpPost]
        public IActionResult AddTeamMembers(AddTeamMemberDto addTeamMemberDto)
        {
            var TeamMemberEntity = new TeamMember()
            {
                FirstName = addTeamMemberDto.FirstName,
                LastName = addTeamMemberDto.LastName,
                Email = addTeamMemberDto.Email,
                Age = addTeamMemberDto.Age,
                EmployeeCode = addTeamMemberDto.EmployeeCode
            };

            dbContext.TeamMembers.Add(TeamMemberEntity); // like persist in symfony
            dbContext.SaveChanges();  //like flush in symfony


            return Ok(TeamMemberEntity);

        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetTeamMemberById(Guid id) // id name should be same as the Route id name above
        {
            var teamMember = dbContext.TeamMembers.Find(id);
            if (teamMember is null)
            {
                return NotFound();
            }
            return Ok(teamMember);

        }

        [HttpPut]
        [Route("{id:guid}")]

        public IActionResult UpdateTeamMember(Guid id, UpdateTeamMemberDto updateTeamMemberDto)
        {
            var teamMember = dbContext.TeamMembers.Find(id);
            // return Ok(id);

            if (teamMember is null)
            {
                return NotFound();
            }

            teamMember.FirstName = updateTeamMemberDto.FirstName;
            teamMember.LastName = updateTeamMemberDto.LastName;
            teamMember.Email = updateTeamMemberDto.Email;
            teamMember.Age = updateTeamMemberDto.Age;
            teamMember.EmployeeCode = updateTeamMemberDto.EmployeeCode;

            dbContext.SaveChanges(); // like flush in symfony

            return Ok(teamMember);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteTeamMember(Guid id)
        {
            var teamMember = dbContext.TeamMembers.Find(id);
            if (teamMember is null)
            {
                return NotFound();
            }

            dbContext.TeamMembers.Remove(teamMember);
            dbContext.SaveChanges();
            return Ok("successfully removed.");
        }

        [HttpGet]
        [Route("search/{search}")]
        public IActionResult GetTeamMemeberByName(string search)
        {
            var searchWord = search.ToString().ToLower();
            var teamMember = dbContext.TeamMembers.Where(x =>
            searchWord.Length >= 3 &&
            (
            x.FirstName.ToLower().Contains(searchWord) ||
            (x.EmployeeCode == searchWord) ||
            (x.LastName == searchWord)
            )
            ).ToList();

            if (teamMember is null)
            {
                return NotFound();
            }
            return Ok(teamMember);
            
        }


    }
}