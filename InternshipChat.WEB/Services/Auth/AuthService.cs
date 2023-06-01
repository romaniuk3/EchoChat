using Blazored.LocalStorage;
using InternshipChat.WEB.Services.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.Models;
using InternshipChat.Shared.DTO.UserDtos;
using System.Text.Json.Serialization;

namespace InternshipChat.WEB.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<RegisterResult> Register(RegisterUserDTO userModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/account/register", userModel);
            if (result.IsSuccessStatusCode)
                return new RegisterResult { Successful = true };

            return new RegisterResult
            {
                Successful = false,
                Errors = new List<string> { "Errors" }
            };
        }

        public async Task<LoginResult> Login(LoginDto loginModel)
        {
            var loginAsJson = JsonSerializer.Serialize(loginModel);
            var response = await _httpClient.PostAsync("api/account/login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                return new LoginResult
                {
                    Successful = false,
                    Error = "Username or password is incorrect."
                };
            }

            var loginResult = JsonSerializer.Deserialize<LoginResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            await _localStorage.SetItemAsync("authToken", loginResult.Token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).NotifyAuthStateChanged();
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);

            return loginResult;
        }

        public async Task<HttpResponseMessage> ChangePassword(ChangePasswordModel model)
        {
            return await _httpClient.PostAsJsonAsync("api/account/change", model);
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
        
        public async Task<string> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }
    }
}
