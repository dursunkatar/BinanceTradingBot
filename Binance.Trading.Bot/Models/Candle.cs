using Binance.Trading.Bot.Helpers;
using Newtonsoft.Json;
using System;

namespace Binance.Trading.Bot.Models
{
    public class Candle
    {
        public DateTime Timestamp { get { return DateTimeHelper.UnixTimestampToDateTime(UnixTimestamp); } }

        [JsonProperty("o")]
        public decimal Open { get; set; }

        [JsonProperty("c")]
        public decimal Close { get; set; }

        [JsonProperty("h")]
        public decimal High { get; set; }

        [JsonProperty("l")]
        public decimal Low { get; set; }

        [JsonProperty("v")]
        public decimal Volume { get; set; }

        [JsonProperty("x")]
        public bool IsClosed { get; set; }

        [JsonProperty("T")]
        public long UnixTimestamp { get; set; }
    }
}
