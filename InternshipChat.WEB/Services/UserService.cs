using Blazored.LocalStorage;
using InternshipChat.DAL.Entities;
using InternshipChat.WEB.Services.Base;
using InternshipChat.WEB.Services.Contracts;

namespace InternshipChat.WEB.Services
{
    public class UserService : BaseHttpService, IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient, ILocalStorageService localStorage) : base(httpClient, localStorage)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            await GetBearerToken();
            return await _httpClient.GetFromJsonAsync<IEnumerable<User>>("api/users/all");
        }
    }
}
