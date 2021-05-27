using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EasySoccer.Entities
{
    public class PersonCompany
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public long CompanyId { get; set; }

        public Guid? PersonId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }
    }
}
