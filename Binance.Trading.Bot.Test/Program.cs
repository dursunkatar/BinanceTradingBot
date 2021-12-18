using System;
using System.Threading.Tasks;

namespace Binance.Trading.Bot.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ReceiverKlineData receiverKlineData = new();
            Task t = receiverKlineData.Deneme(s => Console.WriteLine(s));
            t.Wait();
            Console.ReadLine();
        }
    }
}
