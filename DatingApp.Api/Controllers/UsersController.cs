using DatingApp.Api.Data;
using DatingApp.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Controllers
{
    
    public class UsersController(ApplicationDbContext context) : BaseApiController
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet("")]
        [AllowAnonymous()]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize()]
        public async Task<ActionResult<ApplicationUser>> GetUser([FromRoute] int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
            if (user is null) 
                return NotFound();

            return Ok(user);
        }
    }
}
