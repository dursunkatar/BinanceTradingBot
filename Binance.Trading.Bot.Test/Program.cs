using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Binance.Trading.Bot.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            WSDataReceiver wSDataReceiver = new();
            Task t = wSDataReceiver
                          .SubscribeKline("ethusdt", "btcusdt")
                          .SubscribeAggTrade("ethusdt")
                          .StartReceiver();

            wSDataReceiver.handleDataFunc += s =>
            {

                Regex regex = new Regex("{\"e\":\"(.*?)\",\"");
            
                var v = regex.Match(s);
                Console.WriteLine(v.Groups[1].Value);
            };

            t.Wait();
            Console.ReadLine();
        }
    }
}
