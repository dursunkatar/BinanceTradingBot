using System;
using System.Linq;
using System.Threading.Tasks;
using Binance.Trading.Bot.Enums;
using Binance.Trading.Bot.Models;
using System.Collections.Generic;
using Binance.Trading.Bot.Strategies;

namespace Binance.Trading.Bot.Managers
{
    public struct NotifyTradeManager
    {
        private static HandleTradeSignal onTradeSignal;
        private static readonly List<BaseStrategy> strategies;
        private static readonly Dictionary<string, List<Candle>> symbols;
        private static readonly BinanceWebSocketManager binanceWebSocketManager;
        public delegate void HandleTradeSignal(string symbol, TradeAdvice tradeAdvice, string strategy, DateTime date, decimal ClosePrice);

        static NotifyTradeManager()
        {
            symbols = new();
            strategies = new();
            binanceWebSocketManager = new();
        }

        private async static Task loadLast4hKlineCandlestickDataOfSymbols()
        {
            symbols.Clear();
            var symbolList = await BinanceRestApiManager.getAllSymbols();
            var usdtSymbols = symbolList.Where(x => x.SymbolName.EndsWith("USDT")).ToList();
            for (int i = 0; i < usdtSymbols.Count; i++)
            {
                List<Candle> candles = await BinanceRestApiManager.getLast4hKlineCandlestickData(usdtSymbols[i].SymbolName);
                if (candles != null)
                    symbols.Add(usdtSymbols[i].SymbolName, candles);
            }
        }
        private static void loadStrategies()
        {
            strategies.Add(new RsiMacd());
            strategies.Add(new AdxSmas());
            strategies.Add(new BollingerAwe());
            strategies.Add(new EmaStochRsi());
            strategies.Add(new SarRsi());
            strategies.Add(new SarStoch());
        }
        public async static Task Start(HandleTradeSignal onTradeSignal)
        {
            NotifyTradeManager.onTradeSignal = onTradeSignal;
            loadStrategies();
            await loadLast4hKlineCandlestickDataOfSymbols();
            await binanceWebSocketManager
                   .SubscribeKline(OnKlineDataReceived, symbols.Keys.ToArray())
                   .StartReceiver();
        }
        private static void OnKlineDataReceived(Kline kline)
        {
            bool ok = symbols.TryGetValue(kline.Symbol, out List<Candle> candles);
            if (!ok) return;

            candles.Add(kline.Candle);
            candles.RemoveAt(0);
            for (int i = 0; i < strategies.Count; i++)
            {
                TradeAdvice tradeAdvice = strategies[i].Forecast(candles);
                if (tradeAdvice != TradeAdvice.Hold)
                {
                    onTradeSignal(kline.Symbol, tradeAdvice, strategies[i].Name, kline.Candle.Timestamp, kline.Candle.Close);
                }
            }
        }
    }
}
