using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MyVdsFactory.Application.Common.Mappings;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Earthquakes.Queries.Dtos
{
    public class EarthquakeDto : IMapFrom<Earthquake>
    {
        public long Id { get; set; }
        public double Rms { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Magnitude { get; set; }

        public string Location { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string District { get; set; }

        public DateTime Date { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Earthquake, EarthquakeDto>();
        }
    }
}
