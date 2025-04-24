using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Common;
using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Data;
using DatingApp.Api.Entities;
using DatingApp.Api.Extensions;
using DatingApp.Api.Helpers;
using DatingApp.Api.Interfaces;
using DatingApp.Api.Services.PhotosService;
using DatingApp.Api.Services.UsersService;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DatingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //[ServiceFilter(typeof(LogUserActivity))]
    public class UsersController(IUserService userService, IPhotoService photoService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IPhotoService _photoService = photoService;

        [HttpGet("")]
        public async Task<IActionResult> GetUsers([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
        {
            var username = User.GetUsername();
            filters.CurrentUsername = username;

            var usersResponse = await _userService.GetAllAsync(filters, cancellationToken);

            Response.AddPaginationHeader(usersResponse);

            return Ok(usersResponse.Items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var result = await _userService.GetUserByIdAsync(id);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpGet("username-{username}")]
        public async Task<IActionResult> GetUser([FromRoute] string username)
        {
            var result = await _userService.GetUserByUsernameAsync(username);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var username = User.GetUsername();
            var result = await _userService.UpdateUserByUsernameAsync(username!,request);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }

        [HttpPost("add-photo")]
        public async Task<IActionResult> AddPhoto(IFormFile file)
        {
            var username = User.GetUsername();
            var result = await _userService.AddPhotoToUserAsync(username!, file);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetUser),new { username }, result.Value)
                : result.ToProblem();
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<IActionResult> SetMainPhoto(int photoId)
        {
            var username = User.GetUsername();
            var result = await _userService.SetMainPhotoToUserAsync(username!, photoId);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var username = User.GetUsername();
            var result = await _userService.DeletePhotoFromUserAsync(username!, photoId);

            return result.IsSuccess
                ? Ok()
                : result.ToProblem();
        }

    }
}
