using InternshipChat.Domain.ServiceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.Domain.Errors
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
        }
    }
}
