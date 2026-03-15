using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MYTEAMMANAGER.Controllers
{

    public class  ErrorHandlingController : BaseApiController
    {

        [HttpGet("auth")]
        public IActionResult GetAuth()
        {
            return Unauthorized();
            // throw new UnauthorizedAccessException();
        }

        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            return NotFound();
        }

        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            var number = 0;
            var divi = (float)20/number;
            return Ok( new {divi});
        }

        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {

            return BadRequest();
        }
              
    }
    
}