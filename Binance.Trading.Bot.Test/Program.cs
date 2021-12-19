using Binance.Trading.Bot.Enums;
using Binance.Trading.Bot.Managers;
using Binance.Trading.Bot.Test.DataAccess;
using Binance.Trading.Bot.Test.Entities;
using System;
using System.Threading.Tasks;

namespace Binance.Trading.Bot.Test
{
    class Program
    {
        static void onTradeSignal(string symbol, TradeAdvice tradeAdvice, string strategy, DateTime date)
        {
            using AppDbContext db = new();
            db.TradeSignals.Add(new TradeSignal
            {
                SignalDate = date,
                Strategy = strategy,
                SysmbolName = symbol,
                TradeAdvice = tradeAdvice.ToString()
            });
            db.SaveChanges();
            Console.WriteLine("Symbol:{0} TradeAdvice:{1} Strategy:{2}, Date:{3}", symbol, tradeAdvice, strategy, date);
        }

        static async Task Main(string[] args)
        {
            await NotifyTradeManager.Start(onTradeSignal);

            Console.ReadLine();
        }
    }
}
