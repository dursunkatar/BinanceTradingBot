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

        static async Task Main(string[] args)
        {
            //await NotifyTradeManager.Start(onTradeSignal);
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
            //strategies.Add(new AdxSmas());
            //strategies.Add(new BollingerAwe());
            //strategies.Add(new EmaStochRsi());
            strategies.Add(new SarRsi());
            // strategies.Add(new SarStoch());


            //var candles = db.Candles.ToList();

            //var symbols = await BinanceRestApiManager.getAllSymbols();
            //foreach (var symbol in symbols.Where(s => s.SymbolName.EndsWith("USDT")))
            //{
            //    List<Candle> candles = await BinanceRestApiManager.getLast4hKlineCandlestickData(symbol.SymbolName);
            //    Console.WriteLine(symbol.SymbolName);
            //    Do(candles, strategies, symbol.SymbolName);
            //}

            List<Candle> candles = await BinanceRestApiManager.getLast4hKlineCandlestickData("BTCUSDT");
            Console.WriteLine("BTCUSDT");
            Do(candles, strategies, "BTCUSDT");

            Console.WriteLine("Bitti");
            Console.ReadLine();
        }

        static void Do(List<Candle> candles, List<BaseStrategy> strategies, string symbol)
        {
            using AppDbContext db = new();
            int period = 15;
            for (int i = period; i < candles.Count - (period - 1); i++)
            {
                var _candles = candles.Skip(i).Take(period).ToList();

                Candle lastCandle = _candles[_candles.Count - 1];

                for (int j = 0; j < strategies.Count; j++)
                {
                    string symbol_strategy = $"{symbol}_{strategies[j].Name}";
                    TradeAdvice tradeAdvice = strategies[j].Forecast(_candles);
                    //if (tradeAdvice != TradeAdvice.Hold)
                    //{
                        //if (tradeAdviceLastStatus.ContainsKey(symbol_strategy))
                        //{
                        //    TradeAdvice lastStatus = tradeAdviceLastStatus.GetValueOrDefault(symbol_strategy);
                        //    if (lastStatus == tradeAdvice)
                        //        continue;
                        //}
                        //tradeAdviceLastStatus[symbol_strategy] = tradeAdvice;

                        db.TradeSignals.Add(new TradeSignal
                        {
                            SignalDate = lastCandle.Timestamp,
                            Strategy = strategies[j].Name,
                            SysmbolName = symbol,
                            TradeAdvice = tradeAdvice.ToString(),
                            ClosePrice = lastCandle.Close
                        });
                   // }
                }
            }
            try { db.SaveChanges(); } catch { }

        }
    }
}
