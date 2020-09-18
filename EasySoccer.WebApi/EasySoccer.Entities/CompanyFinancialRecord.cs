using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EasySoccer.Entities
{
    public class CompanyFinancialRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long CompanyId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ExpiresDate { get; set; }

        [Required]
        public decimal Value { get; set; }

        [StringLength(200)]
        public string Transaction { get; set; }

        [Required]
        public bool Paid { get; set; }

        [Required]
        public FinancialPlanEnum FinancialPlan { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
    }
}
