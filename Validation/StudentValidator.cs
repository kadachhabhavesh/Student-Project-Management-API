using FluentValidation;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Validation
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            //RuleFor(student => student.StudentId)
            //    .GreaterThan(0)
            //    .WithMessage("StudentId must be a positive integer.");

            //RuleFor(student => student.UserId)
            //    .NotNull()
            //    .WithMessage("UserId is required.")
            //    .GreaterThan(0)
            //    .WithMessage("UserId must be a positive integer");
                


            RuleFor(student => student.EnrollmentNo)
                .NotEmpty()
                .WithMessage("Enrollment number is required.")
                .MaximumLength(100)
                .WithMessage("Enrollment number cannot exceed 100 characters.");

        }
    }
}
