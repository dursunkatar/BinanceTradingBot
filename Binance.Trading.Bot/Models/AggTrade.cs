using Newtonsoft.Json;

namespace Binance.Trading.Bot.Models
{
    public class AggTrade
    {
        [JsonProperty("e")]
        public string E { get; set; }

        [JsonProperty("E")]
        public long Ee { get; set; }

        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("a")]
        public int A { get; set; }

        [JsonProperty("p")]
        public string P { get; set; }

        [JsonProperty("q")]
        public string Q { get; set; }

        [JsonProperty("f")]
        public int F { get; set; }

        [JsonProperty("l")]
        public int L { get; set; }

        [JsonProperty("T")]
        public long T { get; set; }

        [JsonProperty("m")]
        public bool M { get; set; }

        [JsonProperty("M")]
        public bool Ms { get; set; }
    }
}
