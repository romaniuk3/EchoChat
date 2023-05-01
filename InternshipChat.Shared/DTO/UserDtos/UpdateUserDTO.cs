using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.Shared.DTO.UserDtos
{
    public class UpdateUserDTO : BaseUserDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string? Avatar { get; set; }
        public IFormFile? AvatarImage { get; set; }
    }
}
