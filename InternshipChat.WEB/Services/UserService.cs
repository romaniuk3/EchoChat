using Blazored.LocalStorage;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.DTO.UserDtos;
using InternshipChat.Shared.Models;
using InternshipChat.WEB.Services.Auth;
using InternshipChat.WEB.Services.Base;
using InternshipChat.WEB.Services.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;

namespace InternshipChat.WEB.Services
{
    public class UserService : BaseHttpService, IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly JsonSerializerOptions _options;

        public UserService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider) : base(httpClient, localStorage)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<PagingResponseDTO<UserDTO>> GetUsersAsync(string queryParameters)
        {
            await GetBearerToken();
            var url = "api/users/all" + queryParameters;
            return await _httpClient.GetFromJsonAsync<PagingResponseDTO<UserDTO>>(url, _options);
        }

        public async Task<UserDTO> GetUserAsync(int id)
        {
            await GetBearerToken();
            var uri = $"api/users/{id}";

            return await _httpClient.GetFromJsonAsync<UserDTO>(uri, _options);
        }

        public async Task UpdateUserAsync(int id, UpdateUserDTO updateUserDTO)
        {
            await GetBearerToken();
            var uri = $"api/users/{id}";

            await _httpClient.PutAsJsonAsync(uri, updateUserDTO, _options);
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
