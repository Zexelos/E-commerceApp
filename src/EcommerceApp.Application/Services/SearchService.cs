using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Domain.Interfaces;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace EcommerceApp.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaginatorService<CategoryForListVM> _categoryPaginatorService;
        private readonly IPaginatorService<ProductForListVM> _productPaginatorService;
        private readonly IPaginatorService<EmployeeForListVM> _employeePaginatorService;
        private readonly IPaginatorService<CustomerForListVM> _customerPaginatorService;
        private readonly IPaginatorService<OrderForListVM> _orderPaginatorService;

        public SearchService(
            IMapper mapper,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            IEmployeeRepository employeeRepository,
            ICustomerRepository customerRepository,
            IOrderRepository orderRepository,
            IPaginatorService<EmployeeForListVM> employeePaginatorService,
            IPaginatorService<CustomerForListVM> customerPaginatorService,
            IPaginatorService<CategoryForListVM> categoryPaginatorService,
            IPaginatorService<ProductForListVM> productPaginatorService,
            IPaginatorService<OrderForListVM> orderPaginatorService)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _employeeRepository = employeeRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _employeePaginatorService = employeePaginatorService;
            _customerPaginatorService = customerPaginatorService;
            _categoryPaginatorService = categoryPaginatorService;
            _productPaginatorService = productPaginatorService;
            _orderPaginatorService = orderPaginatorService;
        }

        public async Task<CategoryListVM> SearchPaginatedCategoriesAsync(string selectedValue, string searchString, int pageSize, int pageNumber)
        {
            var parse = int.TryParse(searchString, out int id);
            var emptyQuery = Enumerable.Empty<CategoryForListVM>().AsQueryable();
            var baseQuery = _categoryRepository.GetCategories().ProjectTo<CategoryForListVM>(_mapper.ConfigurationProvider);
            IQueryable<CategoryForListVM> query = selectedValue switch
            {
                "Id" => parse ? baseQuery.Where(x => x.Id == id) : emptyQuery,
                "Name" => baseQuery.Where(x => x.Name.Contains(searchString)),
                _ => emptyQuery
            };
            var paginatedVM = await _categoryPaginatorService.CreateAsync(query, pageNumber, pageSize);
            return _mapper.Map<CategoryListVM>(paginatedVM);
        }

        public async Task<ProductListVM> SearchPaginatedProductsAsync(string selectedValue, string searchString, int pageSize, int pageNumber)
        {
            var idParse = int.TryParse(searchString, out int id);
            var unitPriceParse = decimal.TryParse(searchString, out decimal unitPrice);
            var unitsInStockParse = int.TryParse(searchString, out int unitsInStock);
            var emptyQuery = Enumerable.Empty<ProductForListVM>().AsQueryable();
            var baseQuery = _productRepository.GetProducts().ProjectTo<ProductForListVM>(_mapper.ConfigurationProvider);
            IQueryable<ProductForListVM> query = selectedValue switch
            {
                "Id" => idParse ? baseQuery.Where(x => x.Id == id) : emptyQuery,
                "Name" => baseQuery.Where(x => x.Name.Contains(searchString)),
                "UnitPrice" => unitPriceParse ? baseQuery.Where(x => x.UnitPrice == unitPrice) : emptyQuery,
                "UnitsInStock" => unitsInStockParse ? baseQuery.Where(x => x.UnitsInStock == unitsInStock) : emptyQuery,
                "CategoryName" => baseQuery.Where(x => x.Name.Contains(searchString)),
                _ => emptyQuery
            };
            var paginatedVM = await _productPaginatorService.CreateAsync(query, pageNumber, pageSize);
            return _mapper.Map<ProductListVM>(paginatedVM);
        }

        public async Task<EmployeeListVM> SearchPaginatedEmployeesAsync(string selectedValue, string searchString, int pageSize, int pageNumber)
        {
            var idParse = int.TryParse(searchString, out int id);
            var emptyQuery = Enumerable.Empty<EmployeeForListVM>().AsQueryable();
            var baseQuery = _employeeRepository.GetEmployees().ProjectTo<EmployeeForListVM>(_mapper.ConfigurationProvider);
            IQueryable<EmployeeForListVM> query = selectedValue switch
            {
                "Id" => idParse ? baseQuery.Where(x => x.Id == id) : emptyQuery,
                "FirstName" => baseQuery.Where(x => x.FirstName.Contains(searchString)),
                "LastName" => baseQuery.Where(x => x.LastName.Contains(searchString)),
                "Email" => baseQuery.Where(x => x.Email.Contains(searchString)),
                "Position" => baseQuery.Where(x => x.Position.Contains(searchString)),
                _ => emptyQuery
            };
            var paginatedVM = await _employeePaginatorService.CreateAsync(query, pageNumber, pageSize);
            return _mapper.Map<EmployeeListVM>(paginatedVM);
        }

        public async Task<CustomerListVM> SearchPaginatedCustomersAsync(string selectedValue, string searchString, int pageSize, int pageNumber)
        {
            var idParse = int.TryParse(searchString, out int id);
            var emptyQuery = Enumerable.Empty<CustomerForListVM>().AsQueryable();
            var baseQuery = _customerRepository.GetCustomers().ProjectTo<CustomerForListVM>(_mapper.ConfigurationProvider);
            IQueryable<CustomerForListVM> query = selectedValue switch
            {
                "Id" => idParse ? baseQuery.Where(x => x.Id == id) : emptyQuery,
                "FirstName" => baseQuery.Where(x => x.FirstName.Contains(searchString)),
                "LastName" => baseQuery.Where(x => x.LastName.Contains(searchString)),
                "Email" => baseQuery.Where(x => x.Email.Contains(searchString)),
                "City" => baseQuery.Where(x => x.City.Contains(searchString)),
                "PostalCode" => baseQuery.Where(x => x.PostalCode.Contains(searchString)),
                "Address" => baseQuery.Where(x => x.Address.Contains(searchString)),
                "PhoneNumber" => baseQuery.Where(x => x.PhoneNumber.Contains(searchString)),
                _ => emptyQuery
            };
            var paginatedVM = await _customerPaginatorService.CreateAsync(query, pageNumber, pageSize);
            return _mapper.Map<CustomerListVM>(paginatedVM);
        }

        public async Task<OrderListVM> SearchPaginatedOrdersAsync(string selectedValue, string searchString, int pageSize, int pageNumber)
        {
            var idParse = int.TryParse(searchString, out int id);
            var customerIdParse = int.TryParse(searchString, out int customerId);
            var priceParse = decimal.TryParse(searchString, out decimal price);
            var emptyQuery = Enumerable.Empty<OrderForListVM>().AsQueryable();
            var baseQuery = _orderRepository.GetOrders().ProjectTo<OrderForListVM>(_mapper.ConfigurationProvider);
            IQueryable<OrderForListVM> query = selectedValue switch
            {
                "Id" => idParse ? baseQuery.Where(x => x.Id == id) : emptyQuery,
                "CustomerId" => customerIdParse ? baseQuery.Where(x => x.CustomerId == customerId) : emptyQuery,
                "Price" => priceParse ? baseQuery.Where(x => x.Price == price) : emptyQuery,
                "ShipFirstName" => baseQuery.Where(x => x.ShipFirstName.Contains(searchString)),
                "ShipLastName" => baseQuery.Where(x => x.ShipLastName.Contains(searchString)),
                "ShipCity" => baseQuery.Where(x => x.ShipCity.Contains(searchString)),
                "ShipPostalCode" => baseQuery.Where(x => x.ShipPostalCode.Contains(searchString)),
                "ContactAddress" => baseQuery.Where(x => x.ShipAddress.Contains(searchString)),
                "ContactEmail" => baseQuery.Where(x => x.ContactEmail.Contains(searchString)),
                "ContactPhoneNumber" => baseQuery.Where(x => x.ContactPhoneNumber.Contains(searchString)),
                _ => emptyQuery
            };
            var paginatedVM = await _orderPaginatorService.CreateAsync(query, pageNumber, pageSize);
            return _mapper.Map<OrderListVM>(paginatedVM);
        }
    }
}