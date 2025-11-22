using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IStaffManagementService, StaffManagementService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
