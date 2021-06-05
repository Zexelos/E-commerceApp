using EcommerceApp.Application.ViewModels;
using FluentValidation;

namespace EcommerceApp.Application.Validators
{
    public class CategoryVMValidator : AbstractValidator<CategoryVM>
    {
        public CategoryVMValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MinimumLength(2).MaximumLength(200);
            RuleFor(x => x.FormFileImage).SetValidator(new FileValidator());
        }
    }
}
