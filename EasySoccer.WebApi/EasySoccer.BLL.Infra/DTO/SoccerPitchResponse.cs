using System;
using System.Collections.Generic;

namespace EasySoccer.BLL.Infra.DTO
{
    public class SoccerPitchResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Interval { get; set; }
        public List<AvaliableHour> AvaliableHours { get; set; }
    }
    public class AvaliableHour
    {
        public TimeSpan HourStart { get; set; }
        public TimeSpan HourEnd { get; set; }
    }
}
