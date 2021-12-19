using Binance.Trading.Bot.Indicators;
using Binance.Trading.Bot.Managers;
using Binance.Trading.Bot.Models;
using Binance.Trading.Bot.Strategies;
using Binance.Trading.Bot.Test.DataAccess;
using Binance.Trading.Bot.Test.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Binance.Trading.Bot.Test
{
    class Program
    {
        static void OnKlineDataReceived(Kline kline)
        {

        }

        static void Main(string[] args)
        {
            //BinanceWebSocketManager binanceWebSocketManager = new();
            //_ = binanceWebSocketManager
            //     .SubscribeKline(OnKlineDataReceived, "ethusdt")
            //     .StartReceiver();


            using AppDbContext db = new();
            var candleEntities = db.Candles.Where(x=>x.IsClosed).ToList();

            List<Candle> mumlar = candleEntities.Select(x => new Candle
            {
                Close = x.PriceClose,
                High = x.PriceHigh,
                IsClosed = x.IsClosed,
                Low = x.PriceLow,
                Open = x.PriceOpen,
                Timestamp = x.Timestamp,
                Volume = x.Volume
            }).ToList();

            List<decimal?> rsiResult = mumlar.Rsi();

            Console.WriteLine(rsiResult.Last());
            RsiMacd rsiMacd = new();
            rsiMacd.Prepare(mumlar);



            Console.WriteLine("Bitti");

            Console.ReadLine();
        }
    }
}
