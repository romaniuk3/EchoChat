using FluentValidation.Results;
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
            public static readonly Error IncorrectPassword = new Error(ResultType.Invalid, "Make sure you have provided the correct password.");
        }

        public static class Auth
        {
            public static readonly Error IncorrectData = new Error(ResultType.Invalid, "Incorrect login or password.");
        }

        public static class Attachment
        {
            public static readonly Error NotFound = new Error(ResultType.NotFound, "There is no attachment with given id.");
        }

        public static class Validation
        {
            public static Error ValidationError(List<ValidationFailure> errors) => new Error(ResultType.ValidationErrors, ValidationErrorsToMessages(errors));

            private static List<string> ValidationErrorsToMessages(List<ValidationFailure> errors)
            {
                return errors.Select(e => e.ErrorMessage).ToList();
            }
        }
    }
}
