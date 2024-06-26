﻿using Binance.Trading.Bot.Enums;
using Binance.Trading.Bot.Indicators;
using Binance.Trading.Bot.Models;
using System.Collections.Generic;
using System.Linq;

namespace Binance.Trading.Bot.Strategies
{
    public class RsiMacd : BaseStrategy
    {
        public override string Name => "RSI MACD";
      
        public override TradeAdvice Forecast(List<Candle> candles)
        {
            return Prepare(candles).LastOrDefault();
        }
        public override List<TradeAdvice> Prepare(List<Candle> candles)
        {
            var result = new List<TradeAdvice>();

            var _macd = candles.Macd(24, 52, 18);

            if (!_macd.Macd.Any() || !_macd.Signal.Any())
            {
                result.Add(TradeAdvice.Hold);
                return result;
            }

            var _rsi = candles.Rsi(14);
            for (int i = 0; i < candles.Count; i++)
            {
                if (_rsi[i] > 70 && (_macd.Macd[i] - _macd.Signal[i]) < 0)
                {
                    result.Add(TradeAdvice.Sell);
                   // Console.WriteLine("RSİ: {0}  MACD RESULT: {1}  SIGNAL: {2} Date: {3}  Close: {4}", _rsi[i], (_macd.Macd[i] - _macd.Signal[i]), TradeAdvice.Sell, candles[i].Timestamp, candles[i].Close);
                }
                else if (_rsi[i] < 30 && (_macd.Macd[i] - _macd.Signal[i]) > 0)
                {
                    result.Add(TradeAdvice.Buy);
                    //Console.WriteLine("RSİ: {0}  MACD RESULT: {1}  SIGNAL: {2} Date: {3}  Close: {4}", _rsi[i], (_macd.Macd[i] - _macd.Signal[i]), TradeAdvice.Buy, candles[i].Timestamp, candles[i].Close);
                }

                else
                {
                    result.Add(TradeAdvice.Hold);
                    //Console.WriteLine("RSİ: {0}  MACD RESULT: {1}  SIGNAL: {2} Date: {3}  Close: {4}", _rsi[i], (_macd.Macd[i] - _macd.Signal[i]), TradeAdvice.Hold, candles[i].Timestamp, candles[i].Close);
                }
            }
            return result;
        }
    }
}
