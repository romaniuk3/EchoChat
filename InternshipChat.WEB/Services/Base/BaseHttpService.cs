using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace InternshipChat.WEB.Services.Base
{
    public class BaseHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public BaseHttpService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        protected async Task GetBearerToken()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");

            if (savedToken != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);
            }
        }
    }
}
