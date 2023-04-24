using InternshipChat.DAL.Entities;
using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace InternshipChat.WEB.Helpers
{
    public static class QueryParamParser
    {
        public static T ParseQueryParameter<T> (string uri, string paramName)
        {
            Dictionary<string, string> queryParameters = QueryHelpers.ParseQuery(uri)
                .ToDictionary(x => x.Key, x => x.Value.FirstOrDefault());

            string paramValue;
            if (!queryParameters.TryGetValue(paramName, out paramValue) || string.IsNullOrEmpty(paramValue))
            {
                var userParams = new QueryStringParameters();
                var newParamName = Regex.Replace(paramName, @"\b\p{Ll}", match => match.Value.ToUpper());
                var prop = typeof(QueryStringParameters).GetProperty(newParamName);
                if (prop != null)
                {
                    var defaultValue = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(prop.GetValue(userParams)?.ToString());
                    return defaultValue;
                }

                return default(T);
            }

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

            if (converter == null || !converter.CanConvertFrom(typeof(string)))
            {
                return default(T);
            }

            try
            {
                return (T)converter.ConvertFrom(paramValue);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
