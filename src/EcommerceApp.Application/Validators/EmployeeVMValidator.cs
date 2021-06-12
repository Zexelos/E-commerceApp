using EcommerceApp.Application.ViewModels.AdminPanel;
using FluentValidation;

namespace EcommerceApp.Application.Validators
{
    public class EmployeeVMValidator : AbstractValidator<EmployeeVM>
    {
        public EmployeeVMValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.Position).NotEmpty().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
