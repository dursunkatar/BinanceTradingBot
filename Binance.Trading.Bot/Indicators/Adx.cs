﻿using Binance.Trading.Bot.Helpers;
using Binance.Trading.Bot.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Binance.Trading.Bot.Indicators
{
    public static partial class Extensions
    {
        public static List<decimal?> Adx(this List<Candle> source, int period = 14)
        {
            int outBegIdx, outNbElement;
            double[] adxValues = new double[source.Count];

            var highs = source.Select(x => Convert.ToDouble(x.High)).ToArray();
            var lows = source.Select(x => Convert.ToDouble(x.Low)).ToArray();
            var closes = source.Select(x => Convert.ToDouble(x.Close)).ToArray();

            var adx = TicTacTec.TA.Library.Core.Adx(0, source.Count - 1, highs, lows, closes, period, out outBegIdx, out outNbElement, adxValues);

            if (adx == TicTacTec.TA.Library.Core.RetCode.Success)
            {
                return IndicatorHelper.FixIndicatorOrdering(adxValues.ToList(), outBegIdx, outNbElement);
            }

            throw new Exception("Could not calculate EMA!");
        }
    }
}
