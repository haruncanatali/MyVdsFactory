using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyVdsFactory.Application.Earthquakes.Queries.Dtos;

namespace MyVdsFactory.Application.Earthquakes.Queries.GetEarthquakeList
{
    public class GetEarthquakeListVm
    {
        public List<EarthquakeDto>? Earthquakes { get; set; }
        public long Count { get; set; }
    }
}
