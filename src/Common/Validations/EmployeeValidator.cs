using Common.ViewModels;
using FluentValidation;

namespace Common.Validations
{
    public class EmployeeValidator : AbstractValidator<EmployeeVm>
    {
        public EmployeeValidator()
        {
            RuleFor(d => d.LastName).NotNull().MinimumLength(3).MaximumLength(120).WithMessage("Last Name is required.");
            RuleFor(d => d.FirstName).NotNull().MinimumLength(3).MaximumLength(120).WithMessage("First Name is required.");
            RuleFor(d => d.EmployeeNumber).NotNull().Matches("^(?=.*[a-zA-Z])(?=.*[0-9])[A-Za-z0-9]+$")
                .MinimumLength(5).WithMessage("Must Contain alphanumeric.");
            RuleFor(p => p.HireDate).NotNull().WithMessage("Hire Date is required.");
            RuleFor(p => p.Email).EmailAddress().NotNull().WithMessage("Email is required.");
            RuleFor(p => p.Phone).NotNull().Matches("^[0-9]{10,15}$").WithMessage("Invalid Phone No:");
            RuleFor(p => p.Gender).IsInEnum().NotNull().WithMessage("Gender is required.");
            RuleFor(p => p.Designation).IsInEnum().NotNull().WithMessage("Designation is required.");
            RuleFor(p => p.DepartmentId).NotNull().WithMessage("Department is required.");
            //RuleFor(d => d.Username).NotNull().MinimumLength(8).MaximumLength(120).WithMessage("Username is required.");
        }
    }
}
