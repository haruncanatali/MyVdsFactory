using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MyVdsFactory.Application.Earthquakes.Queries.Dtos;

namespace MyVdsFactory.Application.Earthquakes.Queries.GetEarthquakeList
{
    public class GetEarthquakeListQuery : IRequest<GetEarthquakeListVm>
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
        public string? SortBy { get; set; }
    }
}
