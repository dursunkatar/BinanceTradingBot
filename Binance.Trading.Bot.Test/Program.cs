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
            Console.WriteLine("Kline: " + kline.Candle.Timestamp);
        }

        static void Main(string[] args)
        {

            NotifyTradeManager notifyTradeManager = new();


            Console.ReadLine();
        }
    }
}
