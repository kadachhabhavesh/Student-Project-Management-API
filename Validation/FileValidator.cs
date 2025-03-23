using FluentValidation;

namespace StudentProjectManagementAPI.Validators
{
    public class FileValidator : AbstractValidator<StudentProjectManagementAPI.Models.File>
    {
        public FileValidator()
        {
            RuleFor(f => f.ProjectId)
                .NotNull()
                .WithMessage("Project ID is required.")
                .GreaterThan(0)
                .WithMessage("Project ID must be a positive integer.");

            
            RuleFor(f => f.FileName)
                .NotEmpty()
                .WithMessage("File name is required.")
                .MaximumLength(200)
                .WithMessage("File name cannot exceed 200 characters.");

            
            RuleFor(f => f.FilePath)
                .NotEmpty()
                .WithMessage("File path is required.")
                .MaximumLength(300)
                .WithMessage("File path cannot exceed 300 characters.");

            
            RuleFor(f => f.FileSize)
                .NotNull()
                .WithMessage("File size is required.")
                .GreaterThan(0)
                .WithMessage("File size must be a positive integer.");

            
            //RuleFor(f => f.UploadedAt)
            //    .NotNull()
            //    .WithMessage("UploadedAt timestamp is required.")
            //    .LessThanOrEqualTo(DateTime.Now)
            //    .WithMessage("UploadedAt must be a valid timestamp in the past.");

            
            RuleFor(f => f.UploadedBy)
                .NotNull()
                .WithMessage("UploadedBy is required.")
                .GreaterThan(0)
                .WithMessage("UploadedBy must be a positive integer.");
        }
    }
}
