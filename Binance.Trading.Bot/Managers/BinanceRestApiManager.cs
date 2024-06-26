﻿using Binance.Trading.Bot.Helpers;
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
                    Open = decimal.Parse(item[1].ToString().Replace(".",",")),
                    High = decimal.Parse(item[2].ToString().Replace(".", ",")),
                    Low = decimal.Parse(item[3].ToString().Replace(".", ",")),
                    Close = decimal.Parse(item[4].ToString().Replace(".", ",")),
                    Volume = decimal.Parse(item[5].ToString().Replace(".", ",")),
                    UnixTimestamp = item[6].ToString(),
                    Timestamp = DateTimeHelper.UnixTimestampToDateTime(double.Parse(item[6].ToString())),
                    IsClosed = true
                });
            }
            return candles;
        }
    }
}
