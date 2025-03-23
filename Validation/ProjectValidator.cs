using FluentValidation;
using StudentProjectManagementAPI.Models;
namespace StudentProjectManagementAPI.Validators
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        public ProjectValidator()
        {

            RuleFor(p => p.Title)
                .NotEmpty()
                .WithMessage("Project title is required.")
                .MaximumLength(200)
                .WithMessage("Project title cannot exceed 200 characters.");


            RuleFor(p => p.Description)
                .NotEmpty()
                .WithMessage("Project description is required.");


            RuleFor(p => p.StartDate)
                .NotNull()
                .WithMessage("Start date is required.");


            RuleFor(p => p.EndDate)
                .NotNull()
                .WithMessage("End date is required.")
                .GreaterThanOrEqualTo(p => p.StartDate)
                .WithMessage("End date must be after or equal to the start date.");

            
            RuleFor(p => p.Status)
                .NotEmpty()
                .WithMessage("Status is required.")
                .Must(status => new[] { "Pending", "Active", "Completed" }.Contains(status))
                .WithMessage("Status must be one of the following values: Pending, Active, or Completed.");

            
            RuleFor(p => p.MentorId)
                .NotNull()
                .WithMessage("Mentor ID is required.")
                .GreaterThan(0)
                .WithMessage("Mentor ID must be a positive integer.");
        }
    }
}
