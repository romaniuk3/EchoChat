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
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? Birthdate { get; set; }

        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
