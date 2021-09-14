using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySoccer.Entities
{
    public class SoccerPitchPlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public long CompanyId { get; set; }

        [Required]
        public int Type { get; set; }

        [StringLength(400)]
        public string Description { get; set; }

        public long? IdPlanGenerationConfig { get; set; }

        [Required]
        public bool ShowToUser { get; set; }

        [ForeignKey("IdPlanGenerationConfig")]
        public virtual PlanGenerationConfig PlanGenerationConfig { get; set; }
    }
}
