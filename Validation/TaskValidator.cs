using FluentValidation;
using System.Linq;

namespace StudentProjectManagementAPI.Validators
{
    public class TaskValidator : AbstractValidator<StudentProjectManagementAPI.Models.Task>
    {
        public TaskValidator()
        {
            
            RuleFor(t => t.ProjectId)
                .NotNull()
                .WithMessage("Project ID is required.")
                .GreaterThan(0)
                .WithMessage("Project ID must be a positive integer.");

            
            RuleFor(t => t.Title)
                .NotEmpty()
                .WithMessage("Task title is required.")
                .MaximumLength(200)
                .WithMessage("Task title cannot exceed 200 characters.");

            
            RuleFor(t => t.Description)
                .NotEmpty()
                .WithMessage("Task description is required.");

            
            RuleFor(t => t.Priority)
                .NotEmpty()
                .WithMessage("Priority is required.")
                .Must(p => new[] { "Low", "Medium", "High" }.Contains(p))
                .WithMessage("Priority must be 'Low', 'Medium', or 'High'.");

            
            //RuleFor(t => t.AssignedTo)
            //    .NotNull()
            //    .WithMessage("Assigned To is required.")
            //    .GreaterThan(0)
            //    .WithMessage("Assigned To must be a positive integer.");

            
            RuleFor(t => t.Deadline)
                .NotNull()
                .WithMessage("Deadline is required.")
                .Must(deadline => deadline == null || deadline.Value >= DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Deadline must be a future date.");

            
            RuleFor(t => t.Status)
                .NotEmpty()
                .WithMessage("Status is required.")
                .Must(s => new[] { "Pending", "In Progress", "Completed" }.Contains(s))
                .WithMessage("Status must be 'Pending', 'In Progress', or 'Completed'.");

            
            RuleFor(t => t.CreatedBy)
                .NotNull()
                .WithMessage("Created By is required.")
                .GreaterThan(0)
                .WithMessage("Created By must be a positive integer.");

        }
    }
}
