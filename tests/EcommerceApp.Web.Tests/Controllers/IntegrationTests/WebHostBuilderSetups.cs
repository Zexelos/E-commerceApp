using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public static class WebHostBuilderSetups
    {
        public static HttpClient GetAdminPanelHttpClient(this WebApplicationFactory<Startup> webApplicationFactory)
        {
            return webApplicationFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

                    var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryAdminPanelTest");
                        options.UseInternalServiceProvider(serviceProvider);
                    });

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    try
                    {
                        context.Database.EnsureCreated();
                        context.Employees.Add(
                        new Employee
                        {
                            Id = 1,
                            FirstName = "Test",
                            LastName = "Last",
                            Position = "Position",
                            AppUserId = "123test"
                        });
                        context.Users.Add(
                            new AppUser
                            {
                                Id = "123test",
                                Customer = new Customer
                                {
                                    Id = 1,
                                    FirstName = "ziutek",
                                    LastName = "makowski",
                                    Cart = new Cart
                                    {
                                        CartItems = new List<CartItem>
                                        {
                                            new CartItem
                                            {
                                                Quantity = 2
                                            }
                                        }
                                    }
                                }
                            }
                        );
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new(ex.Message);
                    }
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
    }
}
