using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVdsFactory.Domain.Entities
{
    public class Earthquake : BaseEntity
    {
        public double Rms { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Magnitude { get; set; }

        public string Location { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string District { get; set; }

        public DateTime Date { get; set; }
    }
}
