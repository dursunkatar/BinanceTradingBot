using Binance.Trading.Bot.Indicators;
using Binance.Trading.Bot.Managers;
using Binance.Trading.Bot.Models;
using Binance.Trading.Bot.Strategies;
using Binance.Trading.Bot.Test.DataAccess;
using Binance.Trading.Bot.Test.Entities;
using Binance.Trading.Bot.Test.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Binance.Trading.Bot.Test
{
    class Program
    {
        static void OnKlineDataReceived(Kline kline)
        {
            //if (kline.Candle.IsClosed)
            Console.WriteLine("Symbol: {0} Tarih: {1} IsClosed: {2}", kline.Symbol, kline.Candle.Timestamp, kline.Candle.IsClosed);
        }

        static async Task Main(string[] args)
        {
            var symbolList = await getAllSymbols();
            var l = symbolList.Where(x => x.Symbol.EndsWith("USDT")).Select(x => x.Symbol);

            BinanceWebSocketManager binanceWebSocketManager = new();
            _ = binanceWebSocketManager
                 .SubscribeKline(OnKlineDataReceived, l.ToArray())
                 .StartReceiver();



            //using AppDbContext db = new();
            //var candleEntities = db.Candles.Where(x => x.IsClosed).ToList();

            //List<Candle> mumlar = candleEntities.Select(x => new Candle
            //{
            //    Close = x.PriceClose,
            //    High = x.PriceHigh,
            //    IsClosed = x.IsClosed,
            //    Low = x.PriceLow,
            //    Open = x.PriceOpen,
            //    Timestamp = x.Timestamp,
            //    Volume = x.Volume
            //}).ToList();

            //List<decimal?> rsiResult = mumlar.Rsi();

            ////Console.WriteLine(rsiResult.Last());
            //RsiMacd rsiMacd = new();
            //var s = rsiMacd.Forecast(mumlar);

            //Console.WriteLine(s);

            Console.ReadLine();
        }

        static async Task<List<SymbolModel>> getAllSymbols()
        {
            using HttpClient httpClient = new();
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.binance.com/api/v3/ticker/price");
            var response = await httpClient.SendAsync(request);
            string res = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<SymbolModel>>(res);
        }
    }
}
