using System;
using System.Collections.Generic;

namespace EasySoccer.BLL.Infra.DTO
{
    public class SoccerPitchReservationResponse
    {
        public Guid Id { get; set; }

        public ReservationsGeneratedDTO ChildReservations { get; set; }
    }
}
