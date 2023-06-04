namespace ProductShop.DTOs.Export;

using Newtonsoft.Json;
using ProductShop.Models;

public class ExportProductsInRangeDto
{
    [JsonProperty("name")]
    public string ProductName { get; set; } = null!;

    [JsonProperty("price")]
    public decimal ProductPrice { get; set; }

    [JsonProperty("seller")]
    public string ProductSeller { get; set; } = null!;
}
