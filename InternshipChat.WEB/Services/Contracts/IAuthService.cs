using InternshipChat.Shared.DTO;
using InternshipChat.Shared.DTO.UserDtos;
using InternshipChat.Shared.Models;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IAuthService
    {
        public Task<RegisterResult> Register(RegisterUserDTO userModel);
        public Task<LoginResult> Login(LoginDto loginModel);
        public Task Logout();
        Task<HttpResponseMessage> ChangePassword(ChangePasswordModel model);
        Task<string> GetTokenAsync();
    }
}
