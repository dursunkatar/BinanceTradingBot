using Newtonsoft.Json;

namespace Binance.Trading.Bot.Test.Models
{
    public class SymbolModel
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }
    }
}
