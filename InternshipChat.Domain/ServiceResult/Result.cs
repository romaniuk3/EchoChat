using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.Domain.ServiceResult
{
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        public static Result Success() => new Result(true, Error.None);

        public static Result<TValue> Success<TValue>(TValue value) => new Result<TValue>(value, true, Error.None);

        public static Result Failure(Error error) => new Result(false, error);
        public static Result<TValue> Failure<TValue>(Error error) => new Result<TValue>(default!, false, error);
    }
}
