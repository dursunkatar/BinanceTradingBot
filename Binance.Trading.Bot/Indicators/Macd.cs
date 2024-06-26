﻿using Binance.Trading.Bot.Helpers;
using Binance.Trading.Bot.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Binance.Trading.Bot.Indicators
{
    public static partial class Extensions
    {
        public static MacdItem Macd(this List<Candle> source, int fastPeriod = 12, int slowPeriod = 26, int signalPeriod = 9)
        {
            int outBegIdx, outNbElement;
            double[] macdValues = new double[source.Count];
            double[] signalValues = new double[source.Count];
            double[] histValues = new double[source.Count];
            var closes = source.Select(x => Convert.ToDouble(x.Close)).ToArray();
           
            var macd = TicTacTec.TA.Library.Core.Macd(0, source.Count - 1, closes,
                fastPeriod, slowPeriod, signalPeriod, out outBegIdx, out outNbElement, macdValues, signalValues, histValues);

            if (macd == TicTacTec.TA.Library.Core.RetCode.Success)
            {
                return new MacdItem()
                {
                    Macd = IndicatorHelper.FixIndicatorOrdering(macdValues.ToList(), outBegIdx, outNbElement),
                    Signal = IndicatorHelper.FixIndicatorOrdering(signalValues.ToList(), outBegIdx, outNbElement),
                    Hist = IndicatorHelper.FixIndicatorOrdering(histValues.ToList(), outBegIdx, outNbElement)
                };
            }

            throw new Exception("Could not calculate MACD!");
        }
    }

    public class MacdItem
    {
        public List<decimal?> Macd { get; set; }
        public List<decimal?> Signal { get; set; }
        public List<decimal?> Hist { get; set; }
    }
}
