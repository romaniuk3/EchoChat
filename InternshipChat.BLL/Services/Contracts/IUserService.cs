using InternshipChat.DAL.Entities;
using InternshipChat.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Services.Contracts
{
    public interface IUserService
    {
        public IEnumerable<User> GetAll(UserParameters userParameters);
    }
}
