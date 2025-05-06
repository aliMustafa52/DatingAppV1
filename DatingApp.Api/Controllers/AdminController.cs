using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Services.AdminsService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(IAdminsService adminsService) : ControllerBase
    {
        private readonly IAdminsService _adminsService = adminsService;

        [HttpGet("users-with-roles")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var users = await _adminsService.GetUsersWithRolesAsync();

            return Ok(users);
        }

        [HttpPost("edit-roles/{username}")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<IActionResult> EditRoles([FromRoute] string username,[FromBody] RoleRequest request)
        {
            var result = await _adminsService.EditRolesAsync(username, request);

            return result.IsSuccess
                ? Ok(result.Value) 
                : result.ToProblem();
        }

        [HttpGet("photos-to-moderate")]
        [Authorize(Policy= "ModeratePhotoRole")]
        public IActionResult GetPhotosForModeration()
        {
            return Ok("only admins or moderators here");
        }
    }
}
