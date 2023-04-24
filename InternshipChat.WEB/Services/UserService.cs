using Blazored.LocalStorage;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.Models;
using InternshipChat.WEB.Services.Base;
using InternshipChat.WEB.Services.Contracts;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Identity.Client;
using System.Text.Json;

namespace InternshipChat.WEB.Services
{
    public class UserService : BaseHttpService, IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;

        public UserService(HttpClient httpClient, ILocalStorageService localStorage) : base(httpClient, localStorage)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<PagingResponseDTO<User>> GetUsersAsync(string queryParameters)
        {
            await GetBearerToken();
            //var queryStringParam = GenerateQueryStringParams(userParameters);
            var url = "api/users/all" + queryParameters;
            return await _httpClient.GetFromJsonAsync<PagingResponseDTO<User>>(url, _options);
        }

        public async Task<User> GetUserAsync(int id)
        {
            var uri = $"api/users/{id}";

            return await _httpClient.GetFromJsonAsync<User>(uri, _options);
        }

        public Dictionary<string, string> GenerateQueryStringParams(UserParameters userParameters)
        {
            return new Dictionary<string, string>
            {
                ["pageNumber"] = userParameters.PageNumber.ToString(),
                ["pageSize"] = userParameters.PageSize.ToString(),
                ["searchTerm"] = userParameters.SearchTerm ?? string.Empty,
                ["orderBy"] = userParameters.OrderBy
            };
        }
    }
}
