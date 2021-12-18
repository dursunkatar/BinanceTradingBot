using Binance.Trading.Bot.Enums;
using Binance.Trading.Bot.Indicators;
using Binance.Trading.Bot.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Binance.Trading.Bot.Strategies
{
    public class RsiMacd : BaseStrategy
    {
        public override string Name => "RSI MACD";
        public override int MinimumAmountOfCandles => 52;
        public override Period IdealPeriod => Period.Minute;

        public override TradeAdvice Forecast(List<Candle> candles)
        {
            return Prepare(candles).LastOrDefault();
        }

        protected override List<TradeAdvice> Prepare(List<Candle> candles)
        {
            var result = new List<TradeAdvice>();

            var _macd = candles.Macd(24, 52, 18);

            if (_macd.Macd.Any() || _macd.Signal.Any())
            {
                Console.WriteLine("Macd Boş!");
                return new List<TradeAdvice>();
            }

            var _rsi = candles.Rsi(14);
            for (int i = 0; i < candles.Count; i++)
            {
                if (_rsi[i] > 70 && (_macd.Macd[i] - _macd.Signal[i]) < 0)
                    result.Add(TradeAdvice.Sell);
                else if (_rsi[i] < 30 && (_macd.Macd[i] - _macd.Signal[i]) > 0)
                    result.Add(TradeAdvice.Buy);
                else
                    result.Add(TradeAdvice.Hold);
            }

            return result;
        }
    }
}
