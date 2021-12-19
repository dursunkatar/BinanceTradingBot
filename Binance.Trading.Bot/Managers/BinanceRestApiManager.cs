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

        public static async Task<List<Candle>> getLast4hKlineCandlestickData(string symbol, int day = 14)
        {
            List<Candle> candles = new();
            int limit = (24 / 4) * day;
            string res = await HttpHelper.Get(BaseUrl + $"/klines?symbol={symbol}&interval=4h&limit={limit}");
            var items = JsonConvert.DeserializeObject<List<List<object>>>(res);
            foreach (var item in items)
            {
                candles.Add(new Candle
                {
                    Open = decimal.Parse(item[1].ToString()),
                    High = decimal.Parse(item[2].ToString()),
                    Low = decimal.Parse(item[3].ToString()),
                    Close = decimal.Parse(item[4].ToString()),
                    Volume = decimal.Parse(item[5].ToString()),
                    UnixTimestamp = item[6].ToString(),
                    Timestamp = DateTimeHelper.UnixTimestampToDateTime(double.Parse(item[6].ToString())),
                    IsClosed = true
                });
            }
            return candles;
        }
    }
}
