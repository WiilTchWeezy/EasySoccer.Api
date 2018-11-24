using System.ComponentModel.DataAnnotations;

namespace EasySoccer.Entities
{
    public class SoccerPitchPlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }
    }
}
