using FluentValidation;
using InternshipChat.Shared.DTO.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDTO>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(f => f.FirstName).NotEmpty().Length(2, 30).Must(isValidName).WithMessage("{PropertyName} must contain only letters");
            RuleFor(f => f.LastName).NotEmpty().Length(2, 30).Must(isValidName).WithMessage("{PropertyName} must contain only letters");
            RuleFor(f => f.Email).NotEmpty().EmailAddress();
        }

        private bool isValidName(string name)
        {
            return name.All(char.IsLetter);
        }
    }
}
