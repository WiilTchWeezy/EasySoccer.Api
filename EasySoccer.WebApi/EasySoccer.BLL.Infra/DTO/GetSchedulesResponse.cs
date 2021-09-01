using System;
using System.Collections.Generic;

namespace EasySoccer.BLL.Infra.DTO
{
    public class GetSchedulesResponse
    {
        public int Hour { get; set; }
        public List<GetSchedulesResponseEvents> Events { get; set; }
        public bool AllSoccerPitchesOcupied { get; set; }
        public List<SoccerPitchResponse> FreeSoccerPitches { get; set; }

        public TimeSpan HourSpan { get; set; }
        public GetSchedulesResponse()
        {
            Events = new List<GetSchedulesResponseEvents>();
        }
    }
}
