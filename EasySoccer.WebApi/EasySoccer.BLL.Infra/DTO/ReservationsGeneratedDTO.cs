using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.BLL.Infra.DTO
{
    public class ReservationsGeneratedDTO
    {
        public ReservationsGenerated OriginReservation { get; set; }
        public List<ReservationsGenerated> Reservations { get; set; }
        public List<SoccerPitchReservation> ReservationsEntity { get; set; }
    }

    public class ReservationsGenerated
    {
        public ReservationsGenerated()
        {

        }
        public DateTime SelectedDateStart { get; set; }
        public DateTime SelectedDateEnd { get; set; }
        public int SoccerPitchId { get; set; }
        public string SoccerPitchName { get; set; }
        public int SoccerPitchPlanId { get; set; }
        public string SoccerPitchPlanName { get; set; }
        public bool InsertedSucess { get; set; }
    }
}
