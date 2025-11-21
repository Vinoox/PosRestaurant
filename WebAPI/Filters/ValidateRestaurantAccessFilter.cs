using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class ValidateRestaurantAccessFilter : IAsyncActionFilter
    {
        private readonly ICurrentUserService _currentUserService;

        public ValidateRestaurantAccessFilter(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.TryGetValue("restaurantId", out var restaurantIdObj)
                || restaurantIdObj is not int restaurantIdFromRoute)
            {
                await next();
                return;
            }

            var userRestaurantId = _currentUserService.RestaurantId;


            if (userRestaurantId == null || userRestaurantId != restaurantIdFromRoute)
            {
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }
    }
}