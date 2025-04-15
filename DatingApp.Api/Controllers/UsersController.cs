using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Data;
using DatingApp.Api.Entities;
using DatingApp.Api.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController(IUserRepository userRepository) : BaseApiController
    {
        private readonly IUserRepository _userRepository = userRepository;

        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();

            return Ok(users.Adapt<IEnumerable<UserResponse>>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var result = await _userRepository.GetAsync(id);

            return result.IsSuccess
                ? Ok(result.Value.Adapt<UserResponse>())
                : result.ToProblem();
        }

        [HttpGet("username-{username}")]
        public async Task<IActionResult> GetUser([FromRoute] string username)
        {
            var result = await _userRepository.GetResponseByUsernameAsync(username);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }
    }
}
