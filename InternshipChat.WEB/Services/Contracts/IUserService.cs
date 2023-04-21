using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.Models;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IUserService
    {
        Task<PagingResponseDTO<User>> GetUsersAsync(UserParameters userParameters); 
    }
}
