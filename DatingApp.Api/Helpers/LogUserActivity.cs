using DatingApp.Api.Extensions;
using DatingApp.Api.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingApp.Api.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //code here will be executed before endpoint execute

            var resultContext = await next();

            //code here will be executed after endpoint execute
            if(context.HttpContext.User.Identity?.IsAuthenticated != true)
                return;

            var userId = resultContext.HttpContext.User.GetUserId();

            var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var user = await repo.GetByIdAsync(userId);
            if(user is null) 
                return;

            user.LastActive = DateTime.UtcNow;
            await repo.SaveAllAsync();
        }
    }
}
