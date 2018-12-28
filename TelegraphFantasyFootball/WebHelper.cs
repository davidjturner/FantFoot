using System.Net;

namespace TelegraphFantasyFootball
{
    public class WebHelper
    {
        public HttpWebRequest GetWebRequest(string url, string method, CookieContainer cookieContainer = null, string host = null)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = method;
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.KeepAlive = true;
            webRequest.Host = host ?? "fantasyfootball.telegraph.co.uk";
            webRequest.Accept = "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            webRequest.Headers.Add("Accept-Encoding: gzip, deflate, br");
            webRequest.Headers.Add("Accept-Language: en-US,en;q=0.5");
            webRequest.CookieContainer = cookieContainer ?? new CookieContainer();

            return webRequest;
        }
    }
}
