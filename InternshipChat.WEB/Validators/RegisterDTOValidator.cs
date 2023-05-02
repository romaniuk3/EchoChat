using FluentValidation;
using InternshipChat.Shared.DTO.UserDtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.WEB.Validators
{
    public class RegisterDTOValidator : AbstractValidator<RegisterUserDTO>
    {
        public RegisterDTOValidator() 
        {
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");
            RuleFor(f => f.Email).EmailAddress();
            RuleFor(f => f.FirstName).NotEmpty().Length(2, 30).Must(isValidName).WithMessage("{PropertyName} must contain only letters");
            RuleFor(f => f.LastName).NotEmpty().Length(2, 30).Must(isValidName).WithMessage("{PropertyName} must contain only letters");
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
