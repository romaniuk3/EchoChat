using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.ServiceResult
{
    public sealed class Error
    {
        public Error(ResultType code, string message)
        {
            Code = code;
            Message = message;
        }
        public ResultType Code { get; }
        public string Message { get; }
        internal static Error None => new Error(ResultType.Ok, string.Empty);
    }
}
