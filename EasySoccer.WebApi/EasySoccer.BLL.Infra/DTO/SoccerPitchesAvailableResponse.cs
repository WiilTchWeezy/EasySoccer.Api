using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasySoccer.BLL.Infra.DTO
{
    public class SoccerPitchesAvailableResponse
    {
        public SoccerPitch SoccerPitch { get; set; }
        public bool IsAvaliable { get { return AvaliableStartHours.Any(); } }
        public List<TimeSpan> AvaliableStartHours { get; set; }
        public List<TimeSpan> AvaliableEndHours { get; set; }
        public SoccerPitchesAvailableResponse()
        {
            AvaliableStartHours = new List<TimeSpan>();
            AvaliableEndHours = new List<TimeSpan>();
        }

    }
}
