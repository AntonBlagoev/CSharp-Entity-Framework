namespace CarDealer.DTOs.Import;

using CarDealer.Models;
using Newtonsoft.Json;

public class ImportCarsDto
{
    [JsonProperty("make")]
    public string Make { get; set; } = null!;

    [JsonProperty("model")]
    public string Model { get; set; } = null!;

    [JsonProperty("traveledDistance")]
    public long TraveledDistance { get; set; }

    [JsonProperty("partsId")]
    public int[]? PartsId { get; set; }
}
