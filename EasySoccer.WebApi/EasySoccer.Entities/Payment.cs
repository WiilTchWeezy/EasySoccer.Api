using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EasySoccer.Entities
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public decimal Value { get; set; }
        [Required]
        public Guid SoccerPitchReservationId { get; set; }
        public Guid? PersonCompanyId { get; set; }
        [StringLength(300)]
        public string Note { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public long CompanyUserId { get; set; }
        [Required]
        public int FormOfPaymentId { get; set; }
        [Required]
        public long CompanyId { get; set; }

        [ForeignKey("SoccerPitchReservationId")]
        public virtual SoccerPitchReservation SoccerPitchReservation { get; set; }
        [ForeignKey("PersonCompanyId")]
        public virtual PersonCompany PersonCompany { get; set; }
        [ForeignKey("CompanyUserId")]
        public virtual CompanyUser CompanyUser { get; set; }
        [ForeignKey("FormOfPaymentId")]
        public virtual FormOfPayment FormOfPayment { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
    }
}
