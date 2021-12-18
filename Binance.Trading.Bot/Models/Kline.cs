using Newtonsoft.Json;

namespace Binance.Trading.Bot.Models
{
    public class Kline
    {
        [JsonProperty("s")]
        public string Sysmbol { get; set; }

        [JsonProperty("k")]
        public Candle Candle { get; set; }
    }
}
