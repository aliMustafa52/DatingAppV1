using DatingApp.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet("error")]
        [Authorize()]
        public IActionResult GetError()
        {
            return Ok("secret text");
        }

        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if(thing == null) 
                return NotFound();

            return Ok(thing);
        }

        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            var thing = _context.Users.Find(-1).ToString();
            if (thing == null)
                return NotFound();

            return Ok(thing);
        }

        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {

            return BadRequest("this's not good");
        }
    }
}
