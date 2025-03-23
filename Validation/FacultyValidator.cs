using FluentValidation;
using StudentProjectManagementAPI.Models;
namespace StudentProjectManagementAPI.Validation
{
    public class FacultyValidator : AbstractValidator<Faculty>
    {
        public FacultyValidator()
        {

            //RuleFor(student => student.UserId)
            //    .NotNull()
            //    .WithMessage("UserId is required.")
            //    .GreaterThan(0)
            //    .WithMessage("UserId must be a positive integer");


            RuleFor(f => f.Designation)
                .NotEmpty()
                .WithMessage("Designation is required.")
                .MaximumLength(100)
                .WithMessage("Designation cannot exceed 100 characters.");
        }
    }
}
