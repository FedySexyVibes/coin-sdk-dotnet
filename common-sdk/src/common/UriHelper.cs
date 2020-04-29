using System;
using System.Web;

namespace Coin.Sdk.src.common
{
    public static class UriHelper
    {
        private static readonly char[] _pathsep = new[] { '/' };

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
            builder.Path = $"{builder.Path.TrimEnd(_pathsep)}/{path.TrimStart(_pathsep)}";
            return new Uri(builder.Uri.ToString());
        }
    }
}
