using FluentValidation;
using InternshipChat.Domain.Validators;
using System.Globalization;

namespace InternshipChat.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureValidators(this IServiceCollection services)
        {
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");
            services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UserDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<RegisterDTOValidator>();
        }
    }
}
