using System;
using System.Collections.Generic;
using System.Web;

namespace Coin.Sdk.Common
{
    public static class UriHelper
    {
        public static Uri AddQueryArgs(this Uri url, Dictionary<string, IEnumerable<string>> parameters)
        {
            var builder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(builder.Query);
            foreach (var param in parameters)
            {
                var value = string.Join(",", param.Value);
                if (string.IsNullOrEmpty(value))
                {
                    query.Remove(value);
                }
                else
                {
                    query[param.Key] = string.Join(",", param.Value);
                }
            }
            builder.Query = query.ToString();
            return new Uri(builder.ToString());
        }

        public static Uri AddQueryArg(this Uri url, string paramName, IEnumerable<string> paramValues) =>
            AddQueryArgs(url, new Dictionary<string, IEnumerable<string>> {[paramName] = paramValues});

        public static Uri AddQueryArg(this Uri url, string paramName, string paramValue) =>
            AddQueryArg(url, paramName, new[] {paramValue});

        public static Uri AddPathArg(this Uri url, string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            var builder = new UriBuilder(url);
            builder.Path = $"{builder.Path.TrimEnd('/')}/{path.TrimStart('/')}";
            return new Uri(builder.Uri.ToString());
        }
    }
}