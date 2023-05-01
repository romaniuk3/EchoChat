using FluentValidation;
using InternshipChat.Shared.DTO.UserDtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.Domain.Validators
{
    public class RegisterDTOValidator : AbstractValidator<RegisterUserDTO>
    {
        public RegisterDTOValidator() 
        {
            RuleFor(f => f.Email).EmailAddress();
            RuleFor(f => f.FirstName).NotEmpty().Length(2, 30).Must(isValidName).WithMessage("{PropertyName} must contain only letters");
            RuleFor(f => f.LastName).NotEmpty().Length(2, 30).Must(isValidName).WithMessage("{PropertyName} must contain only letters");
        }

        private bool isValidName(string name)
        {
            return name.All(char.IsLetter);
        }
    }
}
