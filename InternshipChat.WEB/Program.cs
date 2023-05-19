using Azure.Identity;
using Blazored.LocalStorage;
using InternshipChat.WEB.Services;
using InternshipChat.WEB.Services.Auth;
using InternshipChat.WEB.Services.Contracts;
using InternshipChat.WEB.StateContainers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using IMessageService = InternshipChat.WEB.Services.Contracts.IMessageService;

var builder = WebApplication.CreateBuilder(args);
string appBase = string.Empty;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddTransient<IMessageService, MessageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<CallStateContainer>();

if (builder.Environment.IsProduction())
{
    var keyVaultUrl = new Uri(builder.Configuration.GetSection("KeyVaultURL").Value!);
    var azureCredential = new DefaultAzureCredential();
    builder.Configuration.AddAzureKeyVault(keyVaultUrl, azureCredential);
    appBase = builder.Configuration.GetSection("apiappbase").Value!;
} else if (builder.Environment.IsDevelopment())
{
    appBase = builder.Configuration["AppBase"];
}

builder.Services.AddScoped(serviceProvider => new HttpClient
{
    BaseAddress = new Uri(appBase)
});

builder.Services.AddMudServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
