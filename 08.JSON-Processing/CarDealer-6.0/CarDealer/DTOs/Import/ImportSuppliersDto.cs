namespace CarDealer.DTOs.Import;

using Newtonsoft.Json;

public class ImportSuppliersDto
{
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("isImporter")]
    public bool IsImporter { get; set; }
}
