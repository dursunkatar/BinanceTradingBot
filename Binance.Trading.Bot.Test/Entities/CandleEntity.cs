using System;

namespace Binance.Trading.Bot.Test.Entities
{
    public class CandleEntity
    {
        public Int64 CandleId { get; set; }
        public string Symbol { get; set; }
        public decimal PriceOpen { get; set; }
        public decimal PriceClose { get; set; }
        public decimal PriceHigh { get; set; }
        public decimal PriceLow { get; set; }
        public decimal Volume { get; set; }
        public bool IsClosed { get; set; }
        public DateTime CloseTime { get; set; }
        
    }
}
