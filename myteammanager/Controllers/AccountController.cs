using System;
using MYTEAMMANAGER.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using MYTEAMMANAGER.Models.Entities;
using System.Text;
using MYTEAMMANAGER.Models;
using Microsoft.EntityFrameworkCore;
using MYTEAMMANAGER.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;
using MYTEAMMANAGER.Extensions;

namespace MYTEAMMANAGER.Controllers
{
    [Authorize]
    public class AccountController(ApplicationDbContext dbContext) : BaseApiController
    {
        [HttpPost("register-no-form")] // /api/account/register-no-form
        [AllowAnonymous]
        public async Task<ActionResult<TeamMember>> RegisterNoForm(Dictionary<string, string> teamMemberDetails, string sampleName)
        {

            string firstName = teamMemberDetails.TryGetValue("firstName", out string? fName) ? fName : string.Empty;
            string middleName = teamMemberDetails.TryGetValue("middleName", out string? mName) ? mName : string.Empty;
            string lastName = teamMemberDetails.TryGetValue("lastName", out string? lName) ? lName : string.Empty;
            string mobileNumber = teamMemberDetails.TryGetValue("mobileNumber", out string? mNumber) ? mNumber : string.Empty;
            string email = teamMemberDetails.TryGetValue("email", out string? eml) ? eml : string.Empty;
            string userName = teamMemberDetails.TryGetValue("userName", out string? uName) ? uName : string.Empty;
            string employeeCode = teamMemberDetails.TryGetValue("employeeCode", out string? eCode) ? eCode : string.Empty;
            string password = teamMemberDetails.TryGetValue("password", out string? pword) ? pword : string.Empty;
            float age = teamMemberDetails.TryGetValue("age", out string? ag) && float.TryParse(ag, out float a) ? a : 0;

            if (firstName == "")
            {
                return Ok(new { status = "error", msg = "first name cannot be empty" });
            }

            var hmac = new HMACSHA512(); // hmac means hashed message authentication code., sha512 is secure hash algorithm -512 bit(64bytes)

            var teamMember = new TeamMember()
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Age = age,
                MobileNumber = mobileNumber,
                Email = email,
                UserName = userName,
                EmployeeCode = employeeCode,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };


            dbContext.TeamMembers.Add(teamMember);
            await dbContext.SaveChangesAsync();

            return Ok(teamMember);



        }

        [HttpPost("register")] // /api/account/register
        [AllowAnonymous]
        public async Task<ActionResult<TeamMember>> Register(AddTeamMemberDto addTeamMemberDto, ITokenService iTokenService)
        {

            var validationData = await CheckForTheDuplication(addTeamMemberDto.Email, addTeamMemberDto.UserName);
            if (validationData.TryGetValue("status", out string status) && status == "error")
            {
                return BadRequest(validationData);
            }




            var hmac = new HMACSHA512(); // hmac means hashed message authentication code., sha512 is secure hash algorithm -512 bit(64bytes)           
            var teamMember = new TeamMember()
            {
                FirstName = addTeamMemberDto.FirstName,
                MiddleName = addTeamMemberDto.MiddleName,
                LastName = addTeamMemberDto.LastName,
                Age = addTeamMemberDto.Age,
                MobileNumber = addTeamMemberDto.MobileNumber,
                Email = addTeamMemberDto.Email,
                UserName = addTeamMemberDto.UserName,
                EmployeeCode = addTeamMemberDto.EmployeeCode,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(addTeamMemberDto.Password)),
                PasswordSalt = hmac.Key
            };


            dbContext.TeamMembers.Add(teamMember);
            await dbContext.SaveChangesAsync();

            var member = teamMember.ToDto(iTokenService);

            return Ok(member);
        }

        public async Task<Dictionary<string, string>> CheckForTheDuplication(string email, string userName)
        {
            var validationData = new Dictionary<string, string>
            {
                ["status"] = "success"
            };
            // email = email;
            // userName = userName?.Trim().ToLower();
            var duplicateEmailOrUserName = await dbContext.TeamMembers
                .Where(x => x.Email == email || x.UserName == userName)
                .Select(x => new { x.Email, x.UserName })
                .ToListAsync();

            if (duplicateEmailOrUserName.Any(x => x.Email.ToLower() == email))
            {
                validationData["status"] = "error";
                validationData["msg"] = "Email has already registered.";
                return validationData;


            }
            if (duplicateEmailOrUserName.Any(x => x.UserName.ToLower() == userName))
            {
                validationData["status"] = "error";
                validationData["msg"] = "username has already used.";
                return validationData;
            }
            return validationData;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<MemberDto>> LoginTeamMember(LoginTeamMemberDto loginTeamMemberDto, ITokenService iTokenService)
        {
            var userName = loginTeamMemberDto.UserName.Trim();
            var password = loginTeamMemberDto.Password;
            if(userName == "" || password == "")
            {
                throw new Exception("username or password is emtpy");
            }

            var teamMemberDocument = await dbContext.TeamMembers.SingleOrDefaultAsync(x => x.UserName == userName);
            if (teamMemberDocument == null) return Unauthorized("Invalid user name");
            using var hmac = new HMACSHA512(teamMemberDocument.PasswordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (var i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != teamMemberDocument.PasswordHash[i]) return Unauthorized("invalid password");
            }

            var member = teamMemberDocument.ToDto(iTokenService);

            return Ok(member);

        }

        [HttpPost("manager-details")]
        // [AllowAnonymous]
        public async Task<ActionResult<TeamMember>> getManagerDetails([FromHeader] Guid Id)
        {
            var manager = dbContext.TeamMembers.Find(Id) ?? throw new Exception("manager is not found");
            return Ok(manager);
        }
    }
}