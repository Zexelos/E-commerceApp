using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.Order;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace EcommerceApp.Application.Tests.Services.UnitTests
{
    public class OrderServiceUnitTests
    {
        private readonly OrderService _sut;
        private readonly Mock<ICartItemRepository> _cartItemRepository = new();
        private readonly Mock<ICustomerRepository> _customerRepository = new();
        private readonly Mock<IOrderRepository> _orderRepository = new();
        private readonly Mock<IImageConverterService> _imageConverterService = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IPaginatorService<OrderForListVM>> _paginatorServiceOrderForListVM = new();
        private readonly Mock<IPaginatorService<CustomerOrderForListVM>> _paginatorServiceCustomerOrderForListVM = new();
        private readonly Mock<IProductRepository> _productRepository = new();


        public OrderServiceUnitTests()
        {
            _sut = new OrderService(
                _cartItemRepository.Object,
                _customerRepository.Object,
                _orderRepository.Object,
                _imageConverterService.Object,
                _mapper.Object,
                _paginatorServiceOrderForListVM.Object,
                _paginatorServiceCustomerOrderForListVM.Object,
                _productRepository.Object
                );
        }

        [Fact]
        public async Task AddOrderAsync_MethodsRunOnce()
        {
            // Arrange
            var orderCheckoutVM = new OrderCheckoutVM
            {
                CartId = 1,
                TotalPrice = 11.11m,
                CustomerId = 1,
                FirstName = "maciek",
                LastName = "frankowski",
                PostalCode = "123123",
                Address = "makowa 1/1",
                City = "ziutkowo",
                Email = "kekw@kek.com",
                PhoneNumber = "123123123",
                CartItems = new List<CartItemForOrderCheckoutVM>
                {
                    new CartItemForOrderCheckoutVM
                    {
                        ProductId = 1,
                        Name = "frytki",
                        Quantity = 1
                    },
                    new CartItemForOrderCheckoutVM
                    {
                        ProductId = 2,
                        Name = "mleko",
                        Quantity = 2
                    }
                }
            };

            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "frytki",
                    UnitsInStock = 10
                },
                new Product
                {
                    Id = 2,
                    Name = "mleko",
                    UnitsInStock = 10
                }
            };
            var productsQ = products.AsQueryable().BuildMock();

            _productRepository.Setup(s => s.GetProducts()).Returns(productsQ.Object);

            // Act
            await _sut.AddOrderAsync(orderCheckoutVM);

            // Assert
            _productRepository.Verify(v => v.UpdateProductsAsync(It.IsAny<List<Product>>()), Times.Once);
            _orderRepository.Verify(v => v.AddOrderAsync(It.IsAny<Order>()), Times.Once);
            _cartItemRepository.Verify(v => v.DeleteCartItemsByCartIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetOrderCheckoutVMAsync_ReturnsOrderCheckoutVM()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                AppUser = new AppUser
                {
                    Email = "ziutek@kek.com",
                    PhoneNumber = "123123123"
                },
                Cart = new Cart
                {
                    Id = 1,
                    CartItems = new List<CartItem>
                    {
                        new CartItem
                        {
                            Id = 1,
                            Quantity = 2,
                            Product = new Product
                            {
                                Id = 1,
                                Name = "Mleko",
                                UnitPrice = 11.11m,
                                UnitsInStock = 50,
                                Image = new byte[] { 1, 2, 3 }
                            }
                        }
                    }
                }
            };
            var customers = new List<Customer> { customer };
            var customersQ = customers.AsQueryable().BuildMock();

            var cartItems = customer.Cart.CartItems.ToList();

            var orderCheckoutVM = new OrderCheckoutVM
            {
                CustomerId = customer.Id,
                CartId = customer.Cart.Id,
                Email = customer.AppUser.Email,
                PhoneNumber = customer.AppUser.PhoneNumber,
                CartItems = new List<CartItemForOrderCheckoutVM>
                {
                    new CartItemForOrderCheckoutVM
                    {
                        Id = cartItems[0].Id,
                        ProductId = cartItems[0].Product.Id,
                        Name = cartItems[0].Product.Name,
                        Price = cartItems[0].Product.UnitPrice,
                        ProductsInStock = cartItems[0].Product.UnitsInStock,
                        Quantity = cartItems[0].Quantity,
                        ImageToDisplay = "image",
                        ImageByteArray = cartItems[0].Product.Image
                    }
                }
            };

            _customerRepository.Setup(s => s.GetCustomers()).Returns(customersQ.Object);

            _mapper.Setup(s => s.ConfigurationProvider).Returns(
                new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Customer, OrderCheckoutVM>()
                            .ForMember(x => x.CartItems, y => y.MapFrom(src => src.Cart.CartItems))
                            .ForMember(x => x.CustomerId, y => y.MapFrom(src => src.Id))
                            .ForMember(x => x.CartId, y => y.MapFrom(src => src.Cart.Id))
                            .ForMember(x => x.Email, y => y.MapFrom(src => src.AppUser.Email))
                            .ForMember(x => x.PhoneNumber, y => y.MapFrom(src => src.AppUser.PhoneNumber));
                        cfg.CreateMap<CartItem, CartItemForOrderCheckoutVM>()
                            .ForMember(x => x.Name, y => y.MapFrom(src => src.Product.Name))
                            .ForMember(x => x.Price, y => y.MapFrom(src => src.Product.UnitPrice))
                            .ForMember(x => x.ProductsInStock, y => y.MapFrom(src => src.Product.UnitsInStock))
                            .ForMember(x => x.ImageByteArray, y => y.MapFrom(src => src.Product.Image));
                    }
            ));

            _imageConverterService.Setup(s => s.GetImageStringFromByteArray(It.IsAny<byte[]>())).Returns("image");

            // Act
            var result = await _sut.GetOrderCheckoutVMAsync(customer.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderCheckoutVM.Address, result.Address);
            Assert.Equal(orderCheckoutVM.CartItems[0].ImageToDisplay, result.CartItems[0].ImageToDisplay);
            Assert.Equal(orderCheckoutVM.PhoneNumber, result.PhoneNumber);
            Assert.Equal(orderCheckoutVM.CartItems[0].TotalPrice, result.CartItems[0].TotalPrice);
        }

        [Fact]
        public async Task GetPaginatedOrdersAsync_ReturnsOrderListVM()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    ShipFirstName = "maciek",
                    ShipLastName = "wariat",
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Quantity = 2,
                            Product = new Product
                            {
                                Name = "Frytki"
                            }
                        }
                    }
                }
            };
            var ordersQ = orders.AsQueryable().BuildMock();

            var paginatedVM = new PaginatedVM<OrderForListVM>
            {
                Items = new List<OrderForListVM>
                {
                    new OrderForListVM
                    {
                        Id = orders[0].Id,
                        ShipFirstName = orders[0].ShipFirstName,
                        ShipLastName = orders[0].ShipLastName
                    }
                },
                CurrentPage = 1,
                TotalPages = 2
            };

            var orderListVM = new OrderListVM
            {
                Orders = paginatedVM.Items,
                CurrentPage = paginatedVM.CurrentPage,
                TotalPages = paginatedVM.TotalPages
            };

            _orderRepository.Setup(s => s.GetOrders()).Returns(ordersQ.Object);

            _mapper.Setup(s => s.ConfigurationProvider).Returns(
                new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderForListVM>())
            );

            _paginatorServiceOrderForListVM.Setup(s => s.CreateAsync(It.IsAny<IQueryable<OrderForListVM>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(paginatedVM);

            _mapper.Setup(s => s.Map<OrderListVM>(It.IsAny<PaginatedVM<OrderForListVM>>())).Returns(orderListVM);

            // Act
            var result = await _sut.GetPaginatedOrdersAsync(10, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderListVM.CurrentPage, result.CurrentPage);
            Assert.Equal(orderListVM.Orders[0].ShipFirstName, result.Orders[0].ShipFirstName);
        }

        [Fact]
        public async Task GetPaginatedCustomerOrdersAsync_ReturnsCustomerOrderListVM()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    Price = 10m,
                    CreateDate = DateTime.Now,
                    Customer = new Customer
                    {
                        AppUser = new AppUser
                        {
                            Id = "ctwy345cdftwy345"
                        }
                    }
                }
            };
            var ordersQ = orders.AsQueryable().BuildMock();

            var paginatedVM = new PaginatedVM<CustomerOrderForListVM>
            {
                Items = new List<CustomerOrderForListVM>
                {
                    new CustomerOrderForListVM
                    {
                        CreateDate = orders[0].CreateDate,
                        Price = orders[0].Price
                    }
                },
                CurrentPage = 1,
                TotalPages = 2
            };

            var customerOrderListVM = new CustomerOrderListVM
            {
                Orders = paginatedVM.Items,
                CurrentPage = paginatedVM.CurrentPage,
                TotalPages = paginatedVM.TotalPages
            };

            _orderRepository.Setup(s => s.GetOrders()).Returns(ordersQ.Object);

            _mapper.Setup(s => s.ConfigurationProvider).Returns(
                new MapperConfiguration(cfg => cfg.CreateMap<Order, CustomerOrderForListVM>())
            );

            _paginatorServiceCustomerOrderForListVM.Setup(s =>
                s.CreateAsync(It.IsAny<IQueryable<CustomerOrderForListVM>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(paginatedVM);

            _mapper.Setup(s => s.Map<CustomerOrderListVM>(It.IsAny<PaginatedVM<CustomerOrderForListVM>>())).Returns(customerOrderListVM);

            // Act
            var result = await _sut.GetPaginatedCustomerOrdersAsync(orders[0].Customer.AppUser.Id, 10, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customerOrderListVM.CurrentPage, result.CurrentPage);
            Assert.Equal(customerOrderListVM.Orders[0].CreateDate, result.Orders[0].CreateDate);
            Assert.Equal(customerOrderListVM.Orders[0].Price, result.Orders[0].Price);
        }

        [Fact]
        public async Task GetCustomerOrderDetailsAsync_ReturnsCustomerOrderDetailsVM()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    ShipFirstName = "ziutek",
                    Customer = new Customer
                    {
                        AppUserId = "34dt3c3y45",
                        AppUser = new AppUser
                        {
                            Id = "34dt3c3y45"
                        }
                    },
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Quantity = 1,
                            Product = new Product
                            {
                                Name = "mleko",
                                UnitPrice = 10m,
                                Image = new byte[] { 1, 2, 3 }
                            }
                        }
                    }
                }
            };
            var ordersQ = orders.AsQueryable().BuildMock();

            var customerOrderDetailsVM = new CustomerOrderDetailsVM
            {
                Price = 10m,
                ShipFirstName = "ziutek",
                OrderItems = new List<OrderItemForCustomerOrderDetailVM>
                {
                    new OrderItemForCustomerOrderDetailVM
                    {
                        Quantity = 1,
                        ProductName = "mleko",
                        Price = 10m,
                        ImageByteArray = new byte[] { 1, 2, 3 },
                        ImageToDisplay = "image"
                    }
                }
            };

            _orderRepository.Setup(s => s.GetOrders()).Returns(ordersQ.Object);

            _mapper.Setup(s => s.ConfigurationProvider).Returns(
                new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Order, CustomerOrderDetailsVM>()
                        .ForMember(x => x.OrderItems, y => y.MapFrom(src => src.OrderItems));
                    cfg.CreateMap<OrderItem, OrderItemForCustomerOrderDetailVM>()
                        .ForMember(x => x.ProductName, y => y.MapFrom(src => src.Product.Name))
                        .ForMember(x => x.Price, y => y.MapFrom(src => src.Product.UnitPrice))
                        .ForMember(x => x.ImageByteArray, y => y.MapFrom(src => src.Product.Image));
                })
            );

            _imageConverterService.Setup(s => s.GetImageStringFromByteArray(It.IsAny<byte[]>())).Returns("image");

            // Act
            var result = await _sut.GetCustomerOrderDetailsAsync(orders[0].Customer.AppUser.Id, orders[0].Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customerOrderDetailsVM.Price, result.Price);
            Assert.Equal(customerOrderDetailsVM.ShipFirstName, result.ShipFirstName);
            Assert.Equal(customerOrderDetailsVM.OrderItems[0].ProductName, result.OrderItems[0].ProductName);
            Assert.Equal(customerOrderDetailsVM.OrderItems[0].ImageToDisplay, result.OrderItems[0].ImageToDisplay);
        }

        [Fact]
        public async Task GetOrderDetailsAsyncReturnsOrderDetailsVM()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    ShipFirstName = "zordon",
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Quantity = 1,
                            Product = new Product
                            {
                                UnitPrice = 10m,
                                Name = "mleko"
                            }
                        }
                    }
                }
            };
            var ordersQ = orders.AsQueryable().BuildMock();

            var orderDetailsVM = new OrderDetailsVM
            {
                Id = 1,
                ShipFirstName = "zordon",
                OrderItems = new List<OrderItemForDetailsVM>
                {
                    new OrderItemForDetailsVM
                    {
                        Quantity = 1,
                        ProductName = "mleko"
                    }
                }
            };

            _orderRepository.Setup(s => s.GetOrders()).Returns(ordersQ.Object);

            _mapper.Setup(s => s.ConfigurationProvider).Returns(
                new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Order, OrderDetailsVM>()
                        .ForMember(x => x.OrderItems, y => y.MapFrom(src => src.OrderItems));
                    cfg.CreateMap<OrderItem, OrderItemForDetailsVM>()
                        .ForMember(x => x.ProductName, y => y.MapFrom(src => src.Product.Name));
                })
            );

            // Act
            var result = await _sut.GetOrderDetailsAsync(orders[0].Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderDetailsVM.ShipFirstName, result.ShipFirstName);
            Assert.Equal(orderDetailsVM.OrderItems[0].Quantity, result.OrderItems[0].Quantity);
            Assert.Equal(orderDetailsVM.OrderItems[0].ProductName, result.OrderItems[0].ProductName);
        }

        [Fact]
        public async Task DeleteOrderAsync_MethodsRunOnce()
        {
            // Arrange
            int orderId = 10;

            // Act
            await _sut.DeleteOrderAsync(orderId);

            // Assert
            _orderRepository.Verify(v => v.DeleteOrderAsync(It.IsAny<int>()), Times.Once);
        }
    }
}