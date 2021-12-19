using System.Net.Http;
using System.Threading.Tasks;

namespace Binance.Trading.Bot.Helpers
{
    internal struct HttpHelper
    {
        public async static Task<string> Get(string url)
        {
            using HttpClient httpClient = new();
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            httpClient.DefaultRequestHeaders.Add("accept", "application/json");
            var response = await httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
