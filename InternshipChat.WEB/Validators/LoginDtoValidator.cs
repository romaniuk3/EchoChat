using FluentValidation;
using InternshipChat.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.WEB.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");
            RuleFor(f => f.Email).NotEmpty().EmailAddress();
            RuleFor(f => f.Password).NotEmpty().Length(2, 50);
        }
    }
}
