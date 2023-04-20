using InternshipChat.DAL.Entities;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync(); 
    }
}
