using System;

namespace Binance.Trading.Bot.Test.Entities
{
    public class TradeSignal
    {
        public int Id { get; set; }
        public string SysmbolName { get; set; }
        public decimal ClosePrice { get; set; }
        public string TradeAdvice { get; set; }
        public string Strategy { get; set; }
        public DateTime SignalDate { get; set; }
    }
}
