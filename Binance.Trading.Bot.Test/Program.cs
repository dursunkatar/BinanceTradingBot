using Binance.Trading.Bot.Managers;
using Binance.Trading.Bot.Models;
using Binance.Trading.Bot.Test.DataAccess;
using Binance.Trading.Bot.Test.Entities;
using System;

namespace Binance.Trading.Bot.Test
{
    class Program
    {
        static void OnKlineDataReceived(Kline kline)
        {
            using AppDbContext db = new();
            db.Candles.Add(new CandleEntity
            {
                PriceClose = kline.Candle.Close,
                PriceHigh = kline.Candle.High,
                PriceLow = kline.Candle.Low,
                PriceOpen = kline.Candle.Open,
                SymbolId = 1,
                Volume = kline.Candle.Volume,
                Timestamp = kline.Candle.Timestamp,
                IsClosed = kline.Candle.IsClosed
            });
            db.SaveChanges();
        }

        static void Main(string[] args)
        {
            BinanceWebSocketManager binanceWebSocketManager = new();
            _ = binanceWebSocketManager
                 .SubscribeKline(OnKlineDataReceived, "ethusdt")
                 .StartReceiver();

            Console.ReadLine();
        }
    }
}
