using FluentValidation;
using InternshipChat.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(f => f.Email).EmailAddress();
            RuleFor(f => f.Password).NotEmpty().Length(2, 50);
        }
    }
}
