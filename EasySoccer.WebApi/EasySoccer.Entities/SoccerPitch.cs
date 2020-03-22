using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EasySoccer.Entities
{
    public class SoccerPitch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength (300)]
        public string Description { get; set; }

        [Required]
        public bool HasRoof { get; set; }

        public int NumberOfPlayers { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public long CompanyId { get; set; }        

        [Required]
        public bool Active { get; set; }

        public DateTime? ActiveDate { get; set; }

        public DateTime? InactiveDate { get; set; }

        [Required]
        public int SportTypeId { get; set; }

        public int? Interval { get; set; }

        [ForeignKey("SportTypeId")]
        public virtual SportType SportType { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        public virtual ICollection<SoccerPitchSoccerPitchPlan> SoccerPitchSoccerPitchPlans { get; set; }
    }
}
