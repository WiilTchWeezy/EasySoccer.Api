using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasySoccer.BLL.Infra.DTO
{
    public class SoccerPitchesAvailableResponse
    {
        public SoccerPitch SoccerPitch { get; set; }
        public bool IsAvaliable { get { return AvaliableHours.Any(); } }

        public List<AvaliableHour> AvaliableHours { get; set; }
        public SoccerPitchesAvailableResponse()
        {
            AvaliableHours = new List<AvaliableHour>();
        }

    }
}
