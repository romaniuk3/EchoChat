using Blazored.LocalStorage;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.Models;
using InternshipChat.WEB.Services.Base;
using InternshipChat.WEB.Services.Contracts;
using Microsoft.AspNetCore.WebUtilities;
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

        public async Task<PagingResponseDTO<User>> GetUsersAsync(UserParameters userParameters)
        {
            await GetBearerToken();
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = userParameters.PageNumber.ToString(),
                ["pageSize"] = userParameters.PageSize.ToString(),
                ["searchTerm"] = userParameters.SearchTerm ?? "",
                ["orderBy"] = userParameters.OrderBy
            };

            return await _httpClient.GetFromJsonAsync<PagingResponseDTO<User>>(QueryHelpers.AddQueryString("api/users/all", queryStringParam), _options);
        }
    }
}
