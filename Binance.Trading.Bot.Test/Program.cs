using Binance.Trading.Bot.Models;
using System;
using System.Threading.Tasks;

namespace Binance.Trading.Bot.Test
{
    class Program
    {
        static void OnKlineDataReceived(Kline kline)
        {
            Console.WriteLine("Kline: " + kline.Symbol);
        }
  
        static void Main(string[] args)
        {
            WSDataReceiver wSDataReceiver = new();
            Task t = wSDataReceiver
                          .SubscribeKline(OnKlineDataReceived, "ethusdt", "btcusdt")
                          .StartReceiver();

            t.Wait();
            Console.ReadLine();
        }
    }
}
