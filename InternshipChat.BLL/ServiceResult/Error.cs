using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.ServiceResult
{
    public sealed class Error
    {
        public Error(ResultType code, IEnumerable<string> messages)
        {
            Code = code;
            Messages = messages;
        }

        public Error(ResultType code, string message)
        {
            Code = code;
            Messages = new List<string>() { message };
        }

        public ResultType Code { get; }
        public IEnumerable<string> Messages { get; }
        internal static Error None => new(ResultType.Ok, string.Empty);
    }
}
