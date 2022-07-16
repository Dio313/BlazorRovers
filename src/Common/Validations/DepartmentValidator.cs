using Common.ViewModels;
using FluentValidation;

namespace Common.Validations;

public class DepartmentValidator : AbstractValidator<DepartmentVm>
{
    public DepartmentValidator()
    {
        RuleFor(d => d.Name).NotNull().MinimumLength(3).MaximumLength(120).WithMessage("Department name is required.");
        RuleFor(d => d.Description).NotNull().MaximumLength(256).WithMessage("Description name is required.");
    }
}