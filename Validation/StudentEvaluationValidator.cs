using FluentValidation;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Validators
{
    public class StudentEvaluationValidator : AbstractValidator<StudentEvaluation>
    {
        public StudentEvaluationValidator()
        {

            RuleFor(se => se.ProjectId)
                .NotNull()
                .WithMessage("Project ID is required.")
                .GreaterThan(0)
                .WithMessage("Project ID must be a positive integer.");

            
            RuleFor(se => se.FacultyId)
                .NotNull()
                .WithMessage("Faculty ID is required.")
                .GreaterThan(0)
                .WithMessage("Faculty ID must be a positive integer.");

            
            RuleFor(se => se.Score)
                .NotNull()
                .WithMessage("Score is required.")
                .InclusiveBetween(0, 100)
                .WithMessage("Score must be between 0 and 100.");

            
            RuleFor(se => se.Feedback)
                .NotEmpty()
                .WithMessage("Feedback is required.");

            
            RuleFor(se => se.StudentId)
                .NotNull()
                .WithMessage("Student ID is required.")
                .GreaterThan(0)
                .WithMessage("Student ID must be a positive integer.");
        }
    }
}
