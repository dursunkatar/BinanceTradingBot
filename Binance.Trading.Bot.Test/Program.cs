using Binance.Trading.Bot.Models;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Binance.Trading.Bot.Test
{
    class Program
    {
        static void OnKlineDataReceived(Kline kline)
        {

        }
        static void Main(string[] args)
        {
            WSDataReceiver wSDataReceiver = new();
            Task t = wSDataReceiver
                          .SubscribeKline(OnKlineDataReceived, "ethusdt", "btcusdt")
                          .SubscribeAggTrade(s => { }, "ethusdt")
                          .StartReceiver();



            t.Wait();
            Console.ReadLine();
        }
    }
}
