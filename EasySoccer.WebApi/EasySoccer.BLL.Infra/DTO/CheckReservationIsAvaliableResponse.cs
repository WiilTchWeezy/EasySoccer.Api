using EasySoccer.Entities;
using System;

namespace EasySoccer.BLL.Infra.DTO
{
    public class CheckReservationIsAvaliableResponse
    {
        public SoccerPitch SoccerPitch { get; set; }

        public bool IsAvaliable { get; set; }

        public DateTime SelectedDateStart { get; set; }

        public DateTime SelectedDateEnd { get; set; }
    }
}
