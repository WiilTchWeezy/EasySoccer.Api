﻿using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasySoccer.WebApi.ApiRequests
{
    public class SoccerPitchReservationRequestPost
    {
        public long SoccerPitchId { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? PersonCompanyId { get; set; }
        public DateTime SelectedDate { get; set; }
        public TimeSpan HourStart { get; set; }
        public TimeSpan HourEnd { get; set; }
        public int SoccerPitchSoccerPitchPlanId { get; set; }
        public string Note { get; set; }
        public int SoccerPitchPlanId { get; set; }
        public ApplicationEnum Application { get; set; }
    }
}
