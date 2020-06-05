using System;
using System.Web;

namespace Coin.Sdk.Common
{
    public static class UriHelper
    {
        static readonly char[] Pathsep = { '/' };

        public static Uri AddQueryArg(this Uri url, string paramName, string paramValue, bool replace = false)
        {
            var builder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(builder.Query);
            if (replace)
                query[paramName] = paramValue;
            else
                query.Add(paramName, paramValue);
            builder.Query = query.ToString();
            return new Uri(builder.ToString());
        }

        public static Uri AddPathArg(this Uri url, string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            var builder = new UriBuilder(url);
            builder.Path = $"{builder.Path.TrimEnd(Pathsep)}/{path.TrimStart(Pathsep)}";
            return new Uri(builder.Uri.ToString());
        }
    }
}
