using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resutlContext = await next();

            if (!resutlContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resutlContext.HttpContext.User.GetUserId();

            var repo = resutlContext.HttpContext.RequestServices.GetService<IUserRepository>();
            var user = await repo.GetUserByIdAync(userId);
            user.LastActive = DateTime.Now;
            await repo.SaveAllAsync();

        }
    }
}