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
            //var symbolList = await getAllSymbols();
            //var l = symbolList.Where(x => x.Symbol.EndsWith("USDT")).Select(x => x.Symbol);

            //BinanceWebSocketManager binanceWebSocketManager = new();
            //_ = binanceWebSocketManager
            //     .SubscribeKline(OnKlineDataReceived, l.ToArray())
            //     .StartReceiver();




            await NotifyTradeManager.Start(onTradeSignal);


            //using AppDbContext db = new();
            //var candleEntities = db.Candles.Where(x => x.IsClosed).ToList();

            //List<Candle> mumlar = candleEntities.Select(x => new Candle
            //{
            //    Close = x.PriceClose,
            //    High = x.PriceHigh,
            //    IsClosed = x.IsClosed,
            //    Low = x.PriceLow,
            //    Open = x.PriceOpen,
            //    Timestamp = x.Timestamp,
            //    Volume = x.Volume
            //}).ToList();

            //List<decimal?> rsiResult = mumlar.Rsi();

            ////Console.WriteLine(rsiResult.Last());
            //RsiMacd rsiMacd = new();
            //var s = rsiMacd.Forecast(mumlar);

            //Console.WriteLine(s);

            Console.ReadLine();
        }
    }
}
