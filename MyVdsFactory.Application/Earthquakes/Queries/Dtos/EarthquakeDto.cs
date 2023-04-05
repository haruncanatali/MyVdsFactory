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
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Magnitude { get; set; }
        public decimal Depth { get; set; }

        public string Location { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Type { get; set; }

        public DateTime Date { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Earthquake, EarthquakeDto>();
        }
    }
}
