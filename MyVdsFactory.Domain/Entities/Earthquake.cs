using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVdsFactory.Domain.Entities
{
    public class Earthquake : BaseEntity
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Magnitude { get; set; }
        public decimal Depth { get; set; }

        public string Location { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Type { get; set; }

        public DateTime Date { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public long ReferenceId { get; set; }
    }
}
