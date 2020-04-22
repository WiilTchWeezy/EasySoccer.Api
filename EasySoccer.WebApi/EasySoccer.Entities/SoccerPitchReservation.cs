using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EasySoccer.Entities
{
    public class SoccerPitchReservation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public long SoccerPitchId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public int Status { get; set; }

        public long? StatusChangedUserId { get; set; }

        public string Note { get; set; }

        [Required]
        public long SoccerPitchSoccerPitchPlanId { get; set; }

        public Guid? OringinReservationId { get; set; }

        public int? Interval { get; set; }

        [Required]
        public DateTime SelectedDateStart { get; set; }

        [Required]
        public DateTime SelectedDateEnd { get; set; }

        [ForeignKey("SoccerPitchId")]
        public virtual SoccerPitch SoccerPitch { get; set; }

        [ForeignKey("SoccerPitchSoccerPitchPlanId")]
        public virtual SoccerPitchSoccerPitchPlan SoccerPitchSoccerPitchPlan { get; set; }

        [ForeignKey("OringinReservationId")]
        public virtual SoccerPitchReservation OringinReservation { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
