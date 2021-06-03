using System;
using FluentValidation;

namespace EcommerceApp.Application.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public byte[] Picture { get; set; }
    }

    public class ProductVMValidator : AbstractValidator<ProductVM>
    {
        public ProductVMValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MinimumLength(2).MaximumLength(200);
            RuleFor(x => x.UnitPrice).NotEmpty().ScalePrecision(2, 18);
            RuleFor(x => x.UnitsInStock).NotNull();
        }
    }
}
