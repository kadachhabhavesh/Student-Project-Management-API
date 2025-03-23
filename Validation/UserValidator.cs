using FluentValidation;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            //RuleFor(user => user.UserId)
            //    .GreaterThan(0)
            //    .WithMessage("UserId must be greater than 0.");

            
            RuleFor(user => user.FirstName)
                .NotEmpty()
                .WithMessage("FirstName is required.")
                .MaximumLength(100)
                .WithMessage("FirstName cannot exceed 100 characters.");

            
            RuleFor(user => user.LastName)
                .NotEmpty()
                .WithMessage("LastName is required.")
                .MaximumLength(100)
                .WithMessage("LastName cannot exceed 100 characters.");

            
            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .MaximumLength(200)
                .WithMessage("Email cannot exceed 200 characters.");

            
            RuleFor(user => user.PasswordHash)
                .NotEmpty()
                .WithMessage("PasswordHash is required.")
                .MinimumLength(8)
                .WithMessage("PasswordHash must be at least 8 characters.")
                .MaximumLength(300)
                .WithMessage("PasswordHash cannot exceed 300 characters.");

            
            RuleFor(user => user.Role)
                .NotEmpty()
                .WithMessage("Role is required.")
                .Must(role => new[] { "Admin", "Faculty", "Student" }.Contains(role))
                .WithMessage("Role must be one of the following: Admin, Faculty, Student.");


            RuleFor(user => user.Department)
                .NotEmpty()
                .WithMessage("Department is required.")
                .MaximumLength(100)
                .WithMessage("Department cannot exceed 100 characters.");
        }
    }
}
