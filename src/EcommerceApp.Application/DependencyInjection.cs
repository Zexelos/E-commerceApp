using System.Reflection;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddMvc().AddFluentValidation();
            services.AddTransient<IValidator<EmployeeVM>, EmployeeVMValidator>();
            return services;
        }
    }
}