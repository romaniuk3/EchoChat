using InternshipChat.Shared.DTO;
using InternshipChat.WEB.Interfaces;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IAuthService
    {
        public Task<RegisterResult> Register(UserDTO userModel);
        public Task<LoginResult> Login(LoginDto loginModel);
        public Task Logout();
    }
}
