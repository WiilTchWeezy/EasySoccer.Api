using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.BLL.Infra.DTO
{
    public class GetSchedulesResponseEvents
    {
        public string ScheduleHour { get; set; }
        public string SoccerPitch { get; set; }
        public string PersonName { get; set; }
        public bool HasReservation { get; set; }
        public long SoccerPitchId { get; set; }
    }
}
