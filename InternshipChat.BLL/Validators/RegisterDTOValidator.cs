using FluentValidation;
using InternshipChat.Shared.DTO.UserDtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Validators
{
    public class RegisterDTOValidator : AbstractValidator<RegisterUserDTO>
    {
        public RegisterDTOValidator() 
        {
            RuleFor(f => f.Email).NotEmpty().EmailAddress();
            RuleFor(f => f.FirstName).NotEmpty().Length(2, 30).Must(isValidName).WithMessage("{PropertyName} must contain only letters");
            RuleFor(f => f.LastName).NotEmpty().Length(2, 30).Must(isValidName).WithMessage("{PropertyName} must contain only letters");
            RuleFor(f => f.Password)
                .NotEmpty()
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
            RuleFor(f => f.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Password must match.");
            RuleFor(x => x.Birthdate)
                .LessThanOrEqualTo(DateTime.Now.Date)
                .WithMessage("Birth date cannot be in the future.");
            RuleFor(x => x.Birthdate)   
                .Must(date => date <= DateTime.Now.Date.AddYears(-14))
                .WithMessage("You must be at least 14 years old to register.");
        }

        private bool isValidName(string name)
        {
            return name.All(char.IsLetter);
        }
    }
}
