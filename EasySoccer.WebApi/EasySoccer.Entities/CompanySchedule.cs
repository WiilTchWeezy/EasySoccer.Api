using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySoccer.Entities
{
    public class CompanySchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public int Day { get; set; }

        [Required]
        public long StartHour { get; set; }

        [Required]
        public long FinalHour { get; set; }

        [Required]
        public long CompanyId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public int WorkOnThisDay { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
    }
}
