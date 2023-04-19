using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Services.Contracts
{
    public interface IAuthService
    {
        Task<IEnumerable<IdentityError>> Register(UserDTO user);
        Task<LoginResult> Login (LoginDto loginDto);
        Task<ChangePasswordResult> ChangePassword(ChangePasswordModel model);
    }
}
