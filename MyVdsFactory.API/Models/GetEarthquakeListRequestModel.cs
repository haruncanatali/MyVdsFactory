namespace MyVdsFactory.API.Models
{
    public class GetEarthquakeListRequestModel
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Depth { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Magnitude { get; set; }
        public string? Location { get; set; }
        public string? Type { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Sort { get; set; }
    }
}
