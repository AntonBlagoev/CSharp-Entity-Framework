﻿namespace CarDealer.DTOs.Import;

using Newtonsoft.Json;

public class ImportCustomersDto
{
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("birthDate")]

    public DateTime BirthDate { get; set; }

    [JsonProperty("isYoungDriver")]

    public bool IsYoungDriver { get; set; }
}
