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

        [Required]
        public DateTime SelectedDate { get; set; }

        [Required]
        public long SelectedHourStart { get; set; }

        [Required]
        public long SelectedHourEnd { get; set; }

        [ForeignKey("SoccerPitchId")]
        public virtual SoccerPitch SoccerPitch { get; set; }
    }
}
