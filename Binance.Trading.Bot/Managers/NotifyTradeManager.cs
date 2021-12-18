using Binance.Trading.Bot.Enums;
using Binance.Trading.Bot.Models;
using Binance.Trading.Bot.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Binance.Trading.Bot.Managers
{
    public class NotifyTradeManager
    {
        private static volatile object obj = new();
        private static readonly List<Candle> candles;
        private readonly RsiMacd rsiMacd;
        private readonly BinanceWebSocketManager binanceWebSocketManager;

        static NotifyTradeManager()
        {
            candles = new();
        }
        public NotifyTradeManager()
        {
            rsiMacd = new();
            binanceWebSocketManager = new();
            StartReceiver();
        }
        private void StartReceiver()
        {
            _ = binanceWebSocketManager
                   .SubscribeKline(OnKlineDataReceived, "ethusdt")
                   .StartReceiver();
        }
        private void OnKlineDataReceived(Kline kline)
        {
            lock (obj)
            {
                candles.Add(kline.Candle);
                candles.RemoveAll(c => c.Timestamp <= DateTime.Now.AddHours(-1));
                if (candles.Count > 60)
                {
                    TradeAdvice tradeAdvice = rsiMacd.Forecast(candles);
                    Console.WriteLine("Signal: {0}  Zaman: {1}", tradeAdvice, candles[0].Timestamp);
                }
                else
                {
                    Console.WriteLine(candles.Count);
                }
            }
        }
    }
}
