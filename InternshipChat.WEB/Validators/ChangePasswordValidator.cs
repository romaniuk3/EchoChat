using FluentValidation;
using InternshipChat.Shared.Models;
using System.Globalization;

namespace InternshipChat.WEB.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordValidator()
        {
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");
            RuleFor(f => f.CurrentPassword).NotEmpty();
            RuleFor(f => f.NewPassword)
                .NotEmpty()
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
            RuleFor(f => f.ConfirmNewPassword)
                .Equal(x => x.NewPassword).WithMessage("Password must match.");
        }
    }
}
