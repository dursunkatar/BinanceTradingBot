using Binance.Trading.Bot.Enums;
using Binance.Trading.Bot.Models;
using System.Collections.Generic;

namespace Binance.Trading.Bot.Strategies
{
    public abstract class BaseStrategy
    {
        public abstract string Name { get; }
        protected abstract List<TradeAdvice> Prepare(List<Candle> candles);
        public abstract TradeAdvice Forecast(List<Candle> candles);
    }
}
