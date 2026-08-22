using Microsoft.Net.Http.Headers;

namespace PhoenixKC.WebAPI.Shared.Extensions;

public static class HttpCookiesExtensions
{
    public const string CookieHeader = "Set-Cookie";

    extension(HttpResponseMessage thisHttpResponse)
    {
        public string? GetCookie(string key)
        {
            if(!thisHttpResponse.Headers.TryGetValues(CookieHeader, out IEnumerable<string>? cookies))
            {
                return null;
            }
            foreach(string cookieHeader in cookies)
            {
                SetCookieHeaderValue cookie = SetCookieHeaderValue.Parse(cookieHeader);
                if(cookie.Name == key)
                {
                    return cookie.Value.ToString();
                }
            }
            return null;
        }
    }
}