using InternshipChat.Shared.DTO.UserDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.Shared.DTO.UserDtos
{
    public class RegisterUserDTO : BaseUserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? Birthdate { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
