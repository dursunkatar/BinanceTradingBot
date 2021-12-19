using Newtonsoft.Json;
namespace Binance.Trading.Bot.Models
{
    public class Symbol
    {
        [JsonProperty("symbol")]
        public string SymbolName { get; set; }
    }
}
