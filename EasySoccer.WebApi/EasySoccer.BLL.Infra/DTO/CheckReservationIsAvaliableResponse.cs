using EasySoccer.Entities;
using System;

namespace EasySoccer.BLL.Infra.DTO
{
    public class CheckReservationIsAvaliableResponse
    {
        public SoccerPitch SoccerPitch { get; set; }

        public bool IsAvaliable { get; set; }

        public TimeSpan EndHour { get; set; }

        public TimeSpan StartHour { get; set; }
    }
}
