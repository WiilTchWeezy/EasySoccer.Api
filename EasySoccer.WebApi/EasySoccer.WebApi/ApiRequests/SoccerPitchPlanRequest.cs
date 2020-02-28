using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.WebApi.ApiRequests
{
    public class SoccerPitchPlanRequest
    {
        public int id { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }
        public string Description { get; set; }
    }
}
