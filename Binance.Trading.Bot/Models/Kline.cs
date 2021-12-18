using Newtonsoft.Json;

namespace Binance.Trading.Bot.Models
{
    public class Kline
    {
        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("k")]
        public Candle Candle { get; set; }

        [JsonProperty("E")]
        public string UnixTimestamp { get; set; }
    }
}
