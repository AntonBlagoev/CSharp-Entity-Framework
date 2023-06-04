namespace Artillery.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    public class ImportCountryIdDto
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
    }
}
