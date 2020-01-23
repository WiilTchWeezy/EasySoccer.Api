using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.BLL.Infra.DTO
{
    public class AvaliableSchedulesDTO
    {

        public List<SoccerPitch> PossibleSoccerPitchs { get; set; }

        public DateTime SelectedDate { get; set; }

        public TimeSpan SelectedHourStart { get; set; }

        public TimeSpan SelectedHourEnd { get; set; }

        public bool IsCurrentSchedule { get; set; }


    }
}
