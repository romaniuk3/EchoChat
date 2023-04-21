using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.Shared.Models
{
    public class UserParameters : QueryStringParameters
    {
        public UserParameters()
        {
            OrderBy = "email";            
        }
        public string? SearchTerm { get; set; }
        public string? Email { get; set; }
    }
}