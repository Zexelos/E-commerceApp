using System.Reflection;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IEmployeeService, EmployeeService>();
            return services;
        }
    }
}