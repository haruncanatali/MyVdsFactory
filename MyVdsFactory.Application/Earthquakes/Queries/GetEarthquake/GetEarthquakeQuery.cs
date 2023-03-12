using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MyVdsFactory.Application.Earthquakes.Queries.GetEarthquake
{
    public class GetEarthquakeQuery : IRequest<GetEarthquakeVm>
    {
        public long Id { get; set; }
    }
}
