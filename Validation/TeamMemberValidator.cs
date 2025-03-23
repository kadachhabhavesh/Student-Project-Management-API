using FluentValidation;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Validators
{
    public class TeamMemberValidator : AbstractValidator<TeamMember>
    {
        public TeamMemberValidator()
        {

            RuleFor(t => t.ProjectId)
                .NotNull()
                .WithMessage("Project ID is required.")
                .GreaterThan(0)
                .WithMessage("Project ID must be greater than 0.");

            RuleFor(t => t.StudentId)
                .NotNull()
                .WithMessage("Student ID is required.")
                .GreaterThan(0)
                .WithMessage("Student ID must be greater than 0.");

        }
    }
}
