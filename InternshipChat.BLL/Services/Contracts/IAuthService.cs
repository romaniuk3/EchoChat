using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Services.Contracts
{
    public interface IAuthService
    {
        Task<IEnumerable<IdentityError>> Register(UserDTO user);
        Task<AuthResponseDTO?> Login (LoginDto loginDto);
    }
}
