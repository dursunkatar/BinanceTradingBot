using Binance.Trading.Bot.Helpers;
using Binance.Trading.Bot.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
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

        public static async Task<List<Candle>> getLast4hKlineCandlestickData(string symbol, int day = 14)
        {
            List<Candle> candles = new();
            int limit = (24 / 4) * day;
            string res = await HttpHelper.Get(BaseUrl + $"/klines?symbol={symbol}&interval=1M&limit=1000");
            var items = JsonConvert.DeserializeObject<List<List<object>>>(res);
            if (items.Count < limit)
            {
                return null;
            }
            foreach (var item in items)
            {
                candles.Add(new Candle
                {
                    Open = decimal.Parse((string)item[1], CultureInfo.InvariantCulture),
                    High = decimal.Parse((string)item[2], CultureInfo.InvariantCulture),
                    Low = decimal.Parse((string)item[3], CultureInfo.InvariantCulture),
                    Close = decimal.Parse((string)item[4], CultureInfo.InvariantCulture),
                    Volume = decimal.Parse((string)item[5], CultureInfo.InvariantCulture),
                    UnixTimestamp = (long)item[6],
                    IsClosed = true
                });
            }
            return candles;
        }
    }
}
