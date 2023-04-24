using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.Models;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IUserService
    {
        Task<PagingResponseDTO<User>> GetUsersAsync(string queryParameters);
        Dictionary<string, string> GenerateQueryStringParams(UserParameters userParameters);
        Task<User> GetUserAsync(int id);
    }
}
