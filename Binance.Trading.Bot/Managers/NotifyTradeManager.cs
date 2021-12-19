using Binance.Trading.Bot.Enums;
using Binance.Trading.Bot.Models;
using Binance.Trading.Bot.Strategies;
using System.Collections.Generic;

namespace Binance.Trading.Bot.Managers
{
    public struct NotifyTradeManager
    {
        private static readonly List<BaseStrategy> strategies;
        private static readonly Dictionary<string, List<Candle>> symbols;
        private static readonly BinanceWebSocketManager binanceWebSocketManager;

        static NotifyTradeManager()
        {
            symbols = new();
            strategies = new();
            binanceWebSocketManager = new();
            startReceiver();
            loadStrategies();
        }

        private static void loadStrategies()
        {
            strategies.Add(new RsiMacd());
            strategies.Add(new AdxSmas());
        }
        private static void startReceiver()
        {
            _ = binanceWebSocketManager
                   .SubscribeKline(OnKlineDataReceived, "ethusdt")
                   .StartReceiver();
        }
        private static void OnKlineDataReceived(Kline kline)
        {
            bool ok = symbols.TryGetValue(kline.Symbol, out List<Candle> candles);
            if (ok)
            {
                candles.Add(kline.Candle);
                candles.RemoveAt(0);
                for (int i = 0; i < strategies.Count; i++)
                {
                    TradeAdvice tradeAdvice = strategies[i].Forecast(candles);
                }
            }
        }
    }
}
