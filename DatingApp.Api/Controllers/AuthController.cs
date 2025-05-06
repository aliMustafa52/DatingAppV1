using DatingApp.Api.Authentication;
using DatingApp.Api.Contracts.Authentication;
using DatingApp.Api.Data;
using DatingApp.Api.Entities;
using DatingApp.Api.Extensions;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var isUsernameExists = await UserExistsAsync(request.Username);
            if (isUsernameExists)
                return BadRequest("Username is already exists");

            var user = request.Adapt<ApplicationUser>();
            user.UserName = request.Username.ToLower();

            var result = await _userManager.CreateAsync(user, request.Password);
            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var (token, expiresIn) = _jwtProvider.GenerateToken(user, roles);

            var authResponse = new AuthResponse(
                    user.Id,
                    user.UserName,
                    user.KnownAs,
                    token,
                    expiresIn * 60,
                    null,
                    user.Gender);

            return Ok(authResponse);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(x => x.NormalizedUserName == request.Username.ToUpper());
            if (user is null)
                return Unauthorized("Invalid Username");

            var roles = await _userManager.GetRolesAsync(user);
            var (token, expiresIn) = _jwtProvider.GenerateToken(user, roles);

            var isCorrect = await _userManager.CheckPasswordAsync(user, request.Password);
            if(!isCorrect)
                return Unauthorized("Invalid Password");

            var mainPhotoUrl = user.Photos
                .Where(p => p.IsMain)
                .Select(p => p.Url)
                .SingleOrDefault();

            var authResponse = new AuthResponse(
                    user.Id,
                    user.UserName!,
                    user.KnownAs,
                    token,
                    expiresIn * 60,
                    mainPhotoUrl,
                    user.Gender);

            return Ok(authResponse);
        }

        private async Task<bool> UserExistsAsync(string username)
        {
            var isExists = await _userManager.Users.AnyAsync(u => u.UserName!.ToLower() == username.ToLower());
            return isExists;
        }

    }
}
