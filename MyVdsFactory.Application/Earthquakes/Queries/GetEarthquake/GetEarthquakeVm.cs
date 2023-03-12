using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyVdsFactory.Application.Earthquakes.Queries.Dtos;

namespace MyVdsFactory.Application.Earthquakes.Queries.GetEarthquake
{
    public class GetEarthquakeVm
    {
        public EarthquakeDto? Earthquake { get; set; }
    }
}
