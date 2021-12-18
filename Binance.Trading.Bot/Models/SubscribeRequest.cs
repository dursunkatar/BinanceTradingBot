using Newtonsoft.Json;
using System.Collections.Generic;

namespace Binance.Trading.Bot.Models
{
    public class SubscribeRequest
    {
        [JsonProperty("method")]
        public string Method { get; set; } = "SUBSCRIBE";

        [JsonProperty("params")]
        public List<string> Params { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
