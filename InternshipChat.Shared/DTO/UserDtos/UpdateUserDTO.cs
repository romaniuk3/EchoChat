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
        public int Id { get; set; }
        private string _email;
        [Required]
        [EmailAddress]
        public string Email 
        {   get
            {
                return _email;
            } 
            set
            {
                _email = value;
                UserName = value;
                NormalizedEmail = value.ToUpper();
                NormalizedUserName = value.ToUpper();
            }
        }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string NormalizedEmail { get; set; }
    }
}
