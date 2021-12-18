using Binance.Trading.Bot.Enums;
using Binance.Trading.Bot.Models;
using Binance.Trading.Bot.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Binance.Trading.Bot.Managers
{
    public class NotifyTradeManager
    {
        private static volatile object obj = new();
        private static readonly List<Candle> candles;
        private readonly RsiMacd rsiMacd;
        private readonly BinanceWebSocketManager binanceWebSocketManager;

        static NotifyTradeManager()
        {
            candles = new();
        }
        public NotifyTradeManager()
        {
            rsiMacd = new();
            binanceWebSocketManager = new();
            StartReceiver();
        }
        private void StartReceiver()
        {
            _ = binanceWebSocketManager
                   .SubscribeKline(OnKlineDataReceived, "ethusdt")
                   .StartReceiver();
        }
        private void OnKlineDataReceived(Kline kline)
        {
            lock (obj)
            {
                candles.Add(kline.Candle);
                candles.RemoveAll(c => c.Timestamp <= DateTime.Now.AddMinutes(-1));
                TradeAdvice tradeAdvice = rsiMacd.Forecast(candles);
                Console.WriteLine("Signal: {0}  Zaman: {1}", tradeAdvice, candles[0].Timestamp);
            }
        }

        public void GetStrategySignal(BaseStrategy strategy)
        {
            TradeAdvice tradeAdvice = strategy.Forecast(candles);
            Console.WriteLine(tradeAdvice);
        }
        public TradeAdvice _GetStrategySignal(BaseStrategy _strategy)
        {
            try
            {
                var minimumDate = _strategy.GetMinimumDateTime();
                var candleDate = _strategy.GetCurrentCandleDateTime();
                var candles = new List<Candle>();

                // We eliminate all candles that aren't needed for the dataset incl. the last one (if it's the current running candle).
                candles = candles.Where(x => x.Timestamp >= minimumDate && x.Timestamp < candleDate).ToList();

                // Not enough candles to perform what we need to do.
                if (candles.Count < _strategy.MinimumAmountOfCandles)
                    return TradeAdvice.Hold;

                // Get the date for the last candle.
                var signalDate = candles[candles.Count - 1].Timestamp;

                // This is an outdated candle...
                if (signalDate < _strategy.GetSignalDate())
                    return TradeAdvice.Hold;

                // This calculates an advice for the next timestamp.
                return _strategy.Forecast(candles);
            }
            catch (Exception ex)
            {
                return TradeAdvice.Hold;
            }
        }

    }
}
