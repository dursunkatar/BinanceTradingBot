using Binance.Trading.Bot.Enums;
using Binance.Trading.Bot.Managers;
using Binance.Trading.Bot.Models;
using Binance.Trading.Bot.Strategies;
using Binance.Trading.Bot.Test.DataAccess;
using Binance.Trading.Bot.Test.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Binance.Trading.Bot.Test
{
    class Program
    {
        private readonly static Dictionary<string, TradeAdvice> tradeAdviceLastStatus = new();
        static void onTradeSignal(string symbol, TradeAdvice tradeAdvice, string strategy, DateTime date, decimal closePrice)
        {
            if (tradeAdviceLastStatus.ContainsKey(symbol))
            {
                TradeAdvice lastStatus = tradeAdviceLastStatus.GetValueOrDefault(symbol);
                if (lastStatus == tradeAdvice)
                    return;
            }

            tradeAdviceLastStatus[symbol] = tradeAdvice;

            using AppDbContext db = new();
            db.TradeSignals.Add(new TradeSignal
            {
                SignalDate = date,
                Strategy = strategy,
                SysmbolName = symbol,
                TradeAdvice = tradeAdvice.ToString(),
                ClosePrice = closePrice
            });
            db.SaveChanges();
            Console.WriteLine("Symbol:{0} TradeAdvice:{1} Strategy:{2}, Date:{3}", symbol, tradeAdvice, strategy, date);
        }

        static  void Main(string[] args)
        {
            //await NotifyTradeManager.Start(onTradeSignal);
            int[] ss = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //using AppDbContext db = new();
            //List<Candle> candles = await BinanceRestApiManager.getLast4hKlineCandlestickData("ONGUSDT");
            //for (int i = 0; i < candles.Count; i++)
            //{
            //    db.Candles.Add(new CandleEntity
            //    {
            //        CloseTime = candles[i].Timestamp,
            //        IsClosed = true,
            //        PriceClose = candles[i].Close,
            //        PriceHigh = candles[i].High,
            //        PriceLow = candles[i].Low,
            //        PriceOpen = candles[i].Open,
            //        Symbol = "ONGUSDT",
            //        Volume = candles[i].Volume
            //    });
            //}
            //db.SaveChanges();

            List<BaseStrategy> strategies = new();
            strategies.Add(new RsiMacd());
            strategies.Add(new AdxSmas());
            strategies.Add(new BollingerAwe());
            strategies.Add(new EmaStochRsi());
            strategies.Add(new SarRsi());
            strategies.Add(new SarStoch());

            using AppDbContext db = new();
            var candles = db.Candles.ToList();

            int period = 14;
            for (int i = period; i < candles.Count - (period - 1); i++)
            {
                var _candles = candles.Skip(i).Take(period).Select(x => new Candle
                {
                    Close = x.PriceClose,
                    High = x.PriceHigh,
                    IsClosed = x.IsClosed,
                    Low = x.PriceLow,
                    Open = x.PriceOpen,
                    Timestamp = x.CloseTime,
                    Volume = x.Volume,
                }).ToList();

                Candle lastCandle = _candles[_candles.Count - 1];

                for (int j = 0; j < strategies.Count; j++)
                {
                    TradeAdvice tradeAdvice = strategies[j].Forecast(_candles);
                    if (tradeAdvice != TradeAdvice.Hold)
                    {
                        db.TradeSignals.Add(new TradeSignal
                        {
                            SignalDate = lastCandle.Timestamp,
                            Strategy = strategies[j].Name,
                            SysmbolName = "ONGUSDT",
                            TradeAdvice = tradeAdvice.ToString(),
                            ClosePrice = lastCandle.Close
                        });
                    }
                }
            }

            db.SaveChanges();
            Console.WriteLine("Bitti");
            Console.ReadLine();
        }
    }
}
