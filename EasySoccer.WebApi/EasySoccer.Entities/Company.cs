using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySoccer.Entities
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(200)]
        public string CompleteAddress { get; set; }

        [Required]
        public bool Active { get; set; }
        [NotMapped]
        public double Distance { get; set; }

        public virtual ICollection<CompanySchedule> CompanySchedules { get; set; }

    }
}
