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

            var uow = resutlContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
            var user = await uow.UserRepository.GetUserByIdAync(userId);
            user.LastActive = DateTime.UtcNow;
            await uow.Complete();

        }
    }
}