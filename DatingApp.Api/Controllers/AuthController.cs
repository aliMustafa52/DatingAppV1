using DatingApp.Api.Authentication;
using DatingApp.Api.Contracts.Authentication;
using DatingApp.Api.Data;
using DatingApp.Api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ApplicationDbContext context, IJwtProvider jwtProvider) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var isUsernameExists = await UserExistsAsync(request.Username);
            if (isUsernameExists)
                return BadRequest("Username is already exists");

            using var hmac = new HMACSHA512();

            var user = new ApplicationUser
            {
                UserName = request.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
                PasswordSalt = hmac.Key
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var (token, expiresIn) = _jwtProvider.GenerateToken(user);

            var authResponse = new AuthResponse(user.Id, user.UserName, token, expiresIn * 60);
            return Ok(authResponse);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.UserName == request.Username.ToLower());
            if (user is null)
                return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
            for(int i=0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid Password");
            }

            var (token, expiresIn) = _jwtProvider.GenerateToken(user);

            var authResponse = new AuthResponse(user.Id,user.UserName,token,expiresIn * 60);
            return Ok(authResponse);
        }

        private async Task<bool> UserExistsAsync(string username)
        {
            var isExists = await _context.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower());
            return isExists;
        }

    }
}
