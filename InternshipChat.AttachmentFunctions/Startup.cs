using Azure.Identity;
using InternshipChat.AttachmentFunctions;
using InternshipChat.BLL.Services;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

[assembly: FunctionsStartup(typeof(Startup))]
namespace InternshipChat.AttachmentFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var keyVaultUrl = new Uri(Environment.GetEnvironmentVariable("KeyVaultURL"));
            var azureCredential = new DefaultAzureCredential();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddEnvironmentVariables()
                .AddJsonFile("local.settings.json", true)
                .AddAzureKeyVault(keyVaultUrl, azureCredential)
                .Build();

            builder.Services.AddSingleton<IConfiguration>(configuration);

            var databaseConnectionString = configuration.GetSection("azuresqlconnectionstring").Value!;
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddDbContext<ChatContext>(options => options.UseSqlServer(databaseConnectionString));

            builder.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = "ChatAPI",
                    ValidAudience = "ChatAPIClient",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JwtSecretKey")))
                };
            });

            builder.AddAuthorization();
        }
    }
}