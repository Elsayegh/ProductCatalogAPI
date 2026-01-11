using Newtonsoft.Json;

namespace ProductCatalogAPI.Models
{
    public class Product
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("price")]
        public decimal? Price { get; set; }

    }
}
