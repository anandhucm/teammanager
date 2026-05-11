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
using System.Text.Json;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MYTEAMMANAGER.Services;
using MyGrpcService;


namespace MYTEAMMANAGER.Controllers
{
    [Authorize]
    public class AccountController(
        ApplicationDbContext dbContext, 
        IWebHostEnvironment env, 
        IMemberRepository memberRepository,
        IBlobService blobService,
        Greeter.GreeterClient client,
        AzureFunctionsService _azureFunctionsService
        ) : BaseApiController
    {
        
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
            var user = new User()
            {
                FirstName = addTeamMemberDto.FirstName,
                MiddleName = addTeamMemberDto.MiddleName,
                LastName = addTeamMemberDto.LastName,
                UserName = addTeamMemberDto.UserName,
                Email = addTeamMemberDto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(addTeamMemberDto.Password)),
                PasswordSalt = hmac.Key
            };

            var teamMember = new TeamMember()
            {
                FirstName = addTeamMemberDto.FirstName,
                MiddleName = addTeamMemberDto.MiddleName,
                LastName = addTeamMemberDto.LastName,
                Age = addTeamMemberDto.Age,
                MobileNumber = addTeamMemberDto.MobileNumber,
                Email = addTeamMemberDto.Email,
                EmployeeCode = addTeamMemberDto.EmployeeCode,
                DateOfBirth = addTeamMemberDto.DateOfBirth,
                User = user
            };


            dbContext.Users.Add(user);
            dbContext.TeamMembers.Add(teamMember);
            await dbContext.SaveChangesAsync();

            var member = user.ToDto(iTokenService);

            return Ok(member);
        }

        [HttpGet("check-duplication")]
        public async Task<Dictionary<string, string>> CheckForTheDuplication(string email, string userName)
        {
            var validationData = new Dictionary<string, string>
            {
                ["status"] = "success"
            };
            // email = email;
            // userName = userName?.Trim().ToLower();
            var duplicateEmailOrUserName = await dbContext.Users
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

            var teamMemberDocument = await dbContext.Users.SingleOrDefaultAsync(x => x.UserName == userName);
            if (teamMemberDocument == null) return Unauthorized("Invalid user name");
            using var hmac = new HMACSHA512(teamMemberDocument.PasswordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); 
            for (var i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != teamMemberDocument.PasswordHash[i]) return Unauthorized("invalid password");
            }

            var member = teamMemberDocument.ToDto(iTokenService);
            var teamMemberPerson = await dbContext.TeamMembers.FindAsync(teamMemberDocument.Id);
            member.ImageUrl = teamMemberPerson?.ImageUrl;

            return Ok(member);

        }

        [HttpPost("manager-details")]
        // [AllowAnonymous]
        public async Task<ActionResult<TeamMember>> getManagerDetails([FromHeader] Guid Id)
        {
            var manager = dbContext.TeamMembers.Find(Id) ?? throw new Exception("manager is not found");
            return Ok(manager);

        }

        [HttpGet("test-file-operations")]
        [AllowAnonymous]
        public async Task<IActionResult> TestFileOperations(ApplicationDbContext context)
        {


            //env.ContentRootPath give the base root of the project and combining make creation of the absolute path clear.
            var path = Path.Combine(env.ContentRootPath, "Data", "seed-user.json");


            //this way we can get the file info
            var fileInfo = new FileInfo(path);
            var size = fileInfo.Length;
            var fileCreatedTime = fileInfo.CreationTime;



            // for small files we can use this 
            var membersFromFile = await System.IO.File.ReadAllTextAsync(path);



            //for large files(do not use ReadAllText) we use streamReader.
            // if we do not use "using" keyword the stream will not dispose automatically and the file we be locked until we dispose the stream manually(stream.Dispose())
            //using is only applicable for objects implementing IDisposable eg. StreamReader, FileStream, SqlConnection, HttpClient etc
            using var stream = new StreamReader(path);
            var content = await stream.ReadToEndAsync();




            //converting the text json to the c# object of type SeedUserDto.
            var members = JsonSerializer.Deserialize<List<SeedUserDto>>(content);



            var pathWrite = Path.Combine(env.ContentRootPath, "Data", "seed-user-to-write.json");

            if (members != null)
            {              
                foreach (var item in members)
                {
                    if(item.FirstName == "Arun")
                    {
                        var itemText = JsonSerializer.Serialize(item);
                    await System.IO.File.WriteAllTextAsync(pathWrite,itemText);
                    break;
                        
                    }
                }
            }

            return Ok(members);
            
        }

        [HttpGet("members")]
        public async Task<ActionResult<IReadOnlyList<TeamMember>>> getTeamMembersAsync()
        {
            var members = await memberRepository.GetMembersAsync();
            Dictionary<System.Guid, object> userList= [];
            // var i = 1;
            // foreach (var item in members)
            // {
            //     userList[item.Id] = item.FirstName;
            //     i++;             
            // }
            return Ok(members);
        }

        [HttpGet("members/{id}")]
        public async Task<ActionResult<TeamMember>> getTeamMembersAsyncById(string id)
        {
            var member = await memberRepository.getMemberByIdAsync(id);

            if(member == null) return NotFound();

            return member;
            
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetPhotosForMemberAsync(string id)
        {
            var member = await memberRepository.GetPhotosForMemberAsync(id);

            return Ok(member);
            
        }


        [HttpPost("check-grpc")]
        public async Task<ActionResult<UploadResult>> checkGrpc()
        {


            var request =  new HelloRequest { Name = "Exisging App User"};
            var response = await client.SayHelloAsync(request);


            return Ok(response);
            
        }

        [HttpPost("upload-photo")]
        public async Task<ActionResult<UploadResult>> uploadPhoto([FromForm] IFormFile file_details, [FromForm] string id)
        {


            // var request =  new HelloRequest { Name = "Exisging App User"};
            // var response = await client.SayHelloAsync(request);


            var uploadResult =  await blobService.UploadAsync(file_details, id, "");

            if(uploadResult.Status == "success")
            {

                var member = await memberRepository.getMemberByIdAsync(id);
                if(member != null)
                {
                    member.ImageUrl = uploadResult.Message;
                    await dbContext.SaveChangesAsync();
                }
                
            }

            return Ok(uploadResult);
            
        }


        [HttpPost("check-azure-function")]
        public async Task<ActionResult<string>> checheckAzureFunction()
        {

            await _azureFunctionsService.PassWelcomeEmail();
            return Ok(new {Status="success", Message="Email has been send successfully"});
 
        }

    }
}