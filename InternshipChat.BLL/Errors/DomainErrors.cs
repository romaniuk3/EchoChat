using InternshipChat.BLL.ServiceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Errors
{
    public static class DomainErrors
    {
        public static class Chat
        {
            public static readonly Error NotFound = new Error(ResultType.NotFound, "There is no chat with given id.");
            public static readonly Error ChatExists = new Error(ResultType.Invalid, "Chat with given name already exists.");
        }

        public static class User
        {
            public static readonly Error NotFound = new Error(ResultType.NotFound, "There is no user with given id.");
            public static readonly Error UserExists = new Error(ResultType.Invalid, "User with given name already exists.");
        }
    }
}
