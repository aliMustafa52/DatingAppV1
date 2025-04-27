using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Common;
using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Extensions;
using DatingApp.Api.Services.LikesService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikesController(ILikesService likesService) : ControllerBase
    {
        private readonly ILikesService _likesService = likesService;

        [HttpPost("{targetUserId:int}")]
        public async Task<IActionResult> ToggleLike(int targetUserId)
        {
            var userId = User.GetUserId();
            var result = await _likesService.ToggleLikeAsync(userId, targetUserId);

            return result.IsSuccess
                ? Created()
                : result.ToProblem();
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetCurrentUserLikeIds()
        {
            var userId = User.GetUserId();
            var likes = await _likesService.GetCurrentUserLikeIdsAsync(userId);

            return Ok(likes);
        }

        [HttpGet()]
        public async Task<IActionResult> GetUserLikes([FromQuery] RequestLikesFilters requestLikes)
        {
            requestLikes.UserId = User.GetUserId();
            var userResponse = await _likesService.GetUserLikesAsync(requestLikes);

            Response.AddPaginationHeader(userResponse);

            return Ok(userResponse.Items);
        }
    }
}
