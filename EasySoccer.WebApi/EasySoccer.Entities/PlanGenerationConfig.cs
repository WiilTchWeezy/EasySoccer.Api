using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EasySoccer.Entities
{
    public class PlanGenerationConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int IntervalBetweenReservations { get; set; }
        [Required]
        public LimitTypeEnum LimitType { get; set; }
        [Required]
        public int LimitQuantity { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
