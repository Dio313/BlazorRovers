using Common.ViewModels;
using FluentValidation;

namespace Common.Validations
{
    public class ProjectValidator : AbstractValidator<ProjectVm>
     {
        public ProjectValidator()
        {
            RuleFor(p => p.Name).NotNull().MinimumLength(5).WithMessage("Project Name is required.");
            RuleFor(p => p.StartDate).NotNull().WithMessage("Project StartDate is required.");
            RuleFor(p => p.EndDate).NotNull().GreaterThan(c => c.StartDate).WithMessage("Project EndDate Must Greater than StartDate.");
            RuleFor(p => p.Amount).NotNull().ScalePrecision(3,16).WithMessage("Project Amount is required.");
            RuleFor(p => p.EmployeeId).NotNull().WithMessage("Employee is required.");

        }
     }
}
