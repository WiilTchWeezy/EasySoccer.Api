using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySoccer.Entities
{
    public class SoccerPitchPictures
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public long SoccerPitchId { get; set; }

        [ForeignKey("SoccerPitchId")]
        public virtual SoccerPitch SoccerPitch { get; set; }
    }
}
