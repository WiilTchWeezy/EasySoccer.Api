using System;
using System.ComponentModel.DataAnnotations;

namespace EasySoccer.Entities
{
    public class Company
    {
        [Key]
        public long Id { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(15)]
        public string CNPJ { get; set; }

        [StringLength(100)]
        public string Logo { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public bool WorkOnHoliDays { get; set; }
    }
}
