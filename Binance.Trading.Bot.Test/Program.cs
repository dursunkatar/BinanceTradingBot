using Binance.Trading.Bot.Enums;
using Binance.Trading.Bot.Managers;
using Binance.Trading.Bot.Models;
using Binance.Trading.Bot.Strategies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Binance.Trading.Bot.Test
{
    class Program
    {
        static List<Candle> list = new();
        static RsiMacd s = new();
        static void OnKlineDataReceived(Kline kline)
        {
            list.Add(kline.Candle);
            if (list.Count > 150)
            {
                TradeAdvice d = s.Forecast(list);
                Console.WriteLine(d);
            }
            else
            {
                Console.WriteLine("Kline: " + kline.Symbol);
            }
        }

        static void Main(string[] args)
        {
            BinanceWebSocketManager wSDataReceiver = new();
            Task t = wSDataReceiver
                          .SubscribeKline(OnKlineDataReceived, "ethusdt")
                          .StartReceiver();

            t.Wait();
            Console.ReadLine();
        }
    }
}
