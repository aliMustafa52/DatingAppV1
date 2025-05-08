using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Common;
using DatingApp.Api.Contracts.Messages;
using DatingApp.Api.Extensions;
using DatingApp.Api.Services.MessagesService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController(IMessagesService messagesService) : ControllerBase
    {
        private readonly IMessagesService _messagesService = messagesService;

        [HttpPost("")]
        public async Task<IActionResult> CreateMessage(MessageRequest request)
        {
            var username = User.GetUsername();

            var result = await _messagesService.CreateMessage(username!, request);
            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetMessagesForUser([FromQuery] RequestMessageFilters filters)
        {
            filters.Username = User.GetUsername()!;

            var messages = await _messagesService.GetMessagesForUserAsync(filters);

            Response.AddPaginationHeader(messages);

            return Ok(messages.Items);
        }

        [HttpGet("thread/{username}")]
        public async Task<IActionResult> GetMessageThread([FromRoute]string username)
        {
            var currentUsername = User.GetUsername()!;

            var messages = await _messagesService.GetMessageThreadAsync(currentUsername, username);
            return Ok(messages);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();

            var result = await _messagesService.DeleteMessageAsync(username!, id);

            return result.IsSuccess
                ? Ok()
                : result.ToProblem();
        }

    }
}
