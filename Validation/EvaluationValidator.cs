using FluentValidation;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Validation
{
    public class EvaluationValidator : AbstractValidator<Evaluation>
    {
        public EvaluationValidator()
        {
            RuleFor(e => e.ProjectId)
                .NotNull()
                .WithMessage("ProjectId is required.")
                .GreaterThan(0)
                .WithMessage("Project ID, if provided, must be a positive integer.");

            
            RuleFor(e => e.FacultyId)
                .NotNull()
                .WithMessage("ProjectId is required.")
                .GreaterThan(0)
                .WithMessage("Faculty ID, if provided, must be a positive integer.");

            
            RuleFor(e => e.Score)
                .NotNull()
                .WithMessage("Score is required.")
                .InclusiveBetween(0.0, 100.0)
                .WithMessage("Score must be between 0 and 100.");

            
            RuleFor(e => e.Feedback)
                .NotNull()
                .WithMessage("Feedback is required.")
                .NotEmpty()
                .MaximumLength(1000)
                .WithMessage("Feedback cannot exceed 1000 characters.");


            //RuleFor(e => e.EvaluatedAt)
            //    .LessThanOrEqualTo(DateTime.Now)
            //    .WithMessage("EvaluatedAt must be a valid timestamp in the past.")
            //    .NotEmpty()
            //    .WithMessage("EvaluatedAt is required.");
        }
    }
}
