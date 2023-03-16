namespace MyVdsFactory.API.Models
{
    public class GetEarthquakeListRequestModel
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? Date { get; set; }
        public double? Rms { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Magnitude { get; set; }
        public string? Location { get; set; }
        public string? Country { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Sort { get; set; }
    }
}
