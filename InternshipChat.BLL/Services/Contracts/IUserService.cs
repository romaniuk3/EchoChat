using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Helpers;
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
        public PagedList<User> GetAll(UserParameters userParameters);
    }
}
