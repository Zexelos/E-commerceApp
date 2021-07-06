using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure;
using EcommerceApp.Web.Tests.Controllers.IntegrationTests.TestAuthHandlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceApp.Web.Tests.Controllers.IntegrationTests
{
    public static class WebHostBuilderSetups
    {
        public static HttpClient GetGuestHttpClient(this WebApplicationFactory<Startup> webApplicationFactory)
        {
            return webApplicationFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryGuestTest");
                        options.UseInternalServiceProvider(serviceProvider);
                    });

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    try
                    {
                        context.Database.EnsureCreated();
                        context.Categories.Add(
                            new Category
                            {
                                Id = 1,
                                Products = new List<Product>
                                {
                                    new Product
                                    {
                                        Image = new byte[] { 1, 2, 3 }
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

        public static HttpClient GetAdminHttpClient(this WebApplicationFactory<Startup> webApplicationFactory)
        {
            return webApplicationFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, AdminTestAuthHandler>("Test", options => { });

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
                        context.Users.AddRange(
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
                            },
                            new AppUser
                            {
                                Employee = new Employee
                                {
                                    Id = 2,
                                    FirstName = "kaktus",
                                    LastName = "flakowksi"
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

        public static HttpClient GetEmployeeHttpClient(this WebApplicationFactory<Startup> webApplicationFactory)
        {
            return webApplicationFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, EmployeeTestAuthHandler>("Test", options => { });

                    var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryEmployeePanelTest");
                        options.UseInternalServiceProvider(serviceProvider);
                    });

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    try
                    {
                        context.Database.EnsureCreated();
                        context.Orders.Add(
                            new Order
                            {
                                Id = 1,
                                OrderItems = new List<OrderItem>
                                {
                                    new OrderItem
                                    {
                                        Product = new Product
                                        {
                                            Id = 1,
                                            Image = new byte[] { 1, 2, 3 },
                                            Category = new Category
                                            {
                                                Id = 1,
                                                Image = new byte[] { 1, 2, 3 }
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

        public static HttpClient GetCustomerHttpClient(this WebApplicationFactory<Startup> webApplicationFactory)
        {
            return webApplicationFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, CustomerAuthHandler>("Test", options => { });

                    var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryCustomerTest");
                        options.UseInternalServiceProvider(serviceProvider);
                    });

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    try
                    {
                        context.Database.EnsureCreated();
                        context.Add(new AppUser
                        {
                            Id = "Test user",
                            Email = "test@example.com",
                            Customer = new Customer
                            {
                                Id = 1,
                                AppUserId = "Test user",
                                FirstName = "test",
                                LastName = "integration",
                                Cart = new Cart
                                {
                                    Id = 1,
                                    CartItems = new List<CartItem>
                                    {
                                        new CartItem
                                        {
                                            Id = 1,
                                            Quantity = 1,
                                            Product = new Product
                                            {
                                                Id = 1,
                                                Name = "test",
                                                UnitPrice = 1,
                                                Image = new byte[] { 1, 2 },
                                                UnitsInStock = 10,
                                            }
                                        }
                                    }
                                },
                                Orders = new List<Order>
                                {
                                    new Order
                                    {
                                        ShipFirstName = "test"
                                    }
                                }
                            }
                        });
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
