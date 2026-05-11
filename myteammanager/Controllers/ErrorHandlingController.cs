using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MYTEAMMANAGER.Controllers
{

    public class  ErrorHandlingController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        public ErrorHandlingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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
            var TimeNow = DateTime.UtcNow;
            var no = 12;
            var timeZoneId = _configuration["AppSettings:TimeZone"];
            var timeIndia = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(TimeNow, timeZoneId);
            return Ok( new { timeZoneIdFromEnv = timeZoneId, globalTiming = TimeNow, timingIndia = timeIndia  });
        }

        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {

            return BadRequest();
        }
              
    }
    
}