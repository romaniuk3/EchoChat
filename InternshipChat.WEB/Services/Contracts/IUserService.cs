using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.DTO.UserDtos;
using InternshipChat.Shared.Models;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IUserService
    {
        Task<PagingResponseDTO<User>> GetUsersAsync(string queryParameters);
        Dictionary<string, string> GenerateQueryStringParams(UserParameters userParameters);
        Task<User> GetUserAsync(int id);
        Task UpdateUserAsync(int id, UpdateUserDTO updateUserDTO);
    }
}
