using System;
using System.Threading.Tasks;

namespace Binance.Trading.Bot.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            WSDataReceiver wSDataReceiver = new();
            Task t = wSDataReceiver
                          .SubscribeKline("ethusdt","btcusdt")
                          .SubscribeAggTrade("ethusdt")
                          .StartReceiver();

            wSDataReceiver.handleDataFunc += s => Console.WriteLine(s);

            t.Wait();
            Console.ReadLine();
        }
    }
}
