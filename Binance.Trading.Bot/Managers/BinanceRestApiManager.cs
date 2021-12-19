using Binance.Trading.Bot.Helpers;
using Binance.Trading.Bot.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Binance.Trading.Bot.Managers
{
    public class BinanceRestApiManager
    {
        private static readonly string BaseUrl = "https://api.binance.com/api/v3";

        public static async Task<List<Symbol>> getAllSymbols()
        {
            string res = await HttpHelper.Get(BaseUrl + "/ticker/price");
            return JsonConvert.DeserializeObject<List<Symbol>>(res);
        }

        public static async Task<List<List<object>>> getLast4hKlineCandlestickData(int day = 14)
        {
            int limit = (24 / 4) * day;
            string res = await HttpHelper.Get(BaseUrl + $"/klines?symbol=ETHUSDT&interval=4h&limit={limit}");
            var h = JsonConvert.DeserializeObject<List<List<object>>>(res);
            return h;
        }
    }
}
