using System;                 //basic .net types like exception, datatime.
using MYTEAMMANAGER.Interfaces;
using MYTEAMMANAGER.Models.Entities;
using Microsoft.IdentityModel.Tokens; // provides classes for token creation, singining and cryptography - eg. SymmetricSecurityKey, SigningCredentials 
using System.Text; //needed for Encoding.UTF8 
using System.Security.Claims; //provies Claim, ClaimTypes, ClaimsIdentity. - claims are the information you put inside the token.
using System.IdentityModel.Tokens.Jwt; //contains JwtSecurityTokenHandler, which can create, write and validate Jwt tokens.

namespace MYTEAMMANAGER.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(User user)
        {
            /*
             config usually come from appsettings.json or environmental variables.
            */
            var tokenKey = config["TokenKey"] ?? throw new Exception("cannot get the token key");
            if (tokenKey.Length < 64) throw new Exception("token character cannot be less than 64.");

            /*
                SymmetricSecurityKey converts the string tokeyKey to bytes. it represents the key in a format that .net can use for signing tokens.
                symmetric key use same key to sign and validate token (opposite to asymetric keys -  which use public and privat pairs)
            */
            var convertedTokenKey = Encoding.UTF8.GetBytes(tokenKey);
            var key = new SymmetricSecurityKey(convertedTokenKey);


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            /*  
            claim is a peace of information about the user stored in the token.
            [
                { "type": "nameid", "value": "101" },
                { "type": "userName", "value": "Anandhu" }
            ]
            */

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new ("userName", user.UserName),
                new (ClaimTypes.Role, user.FirstName),
                new ("LastName", user.LastName)
            };


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}