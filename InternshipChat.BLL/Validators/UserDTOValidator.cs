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
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(f => f.Email).EmailAddress();
            RuleFor(f => f.FirstName).NotEmpty().Length(2, 20);
            RuleFor(f => f.LastName).NotEmpty().Length(2, 20);
            RuleFor(f => f.Birhdate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("You must be older to use this app.");
        }
    }
}
