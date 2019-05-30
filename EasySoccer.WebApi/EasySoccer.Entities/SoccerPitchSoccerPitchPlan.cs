using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySoccer.Entities
{
    public class SoccerPitchSoccerPitchPlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long SoccerPitchId { get; set; }

        [Required]
        public int SoccerPitchPlanId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}