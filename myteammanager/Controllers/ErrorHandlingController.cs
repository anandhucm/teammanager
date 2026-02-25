using System;
using Microsoft.AspNetCore.Mvc;

namespace MYTEAMMANAGER.Controllers
{

    public class  ErrorHandlingController : BaseApiController
    {

        [HttpGet("auth")]
        public IActionResult GetAuth()
        {
            return Unauthorized();
        }

        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            throw new Exception("This is a server error");
        }

        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {
            throw new Exception("This is not a good request");
        }
              
    }
    
}