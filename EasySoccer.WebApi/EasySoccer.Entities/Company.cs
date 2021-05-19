
using GeoAPI.Geometries;
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

        public int? IdCity { get; set; }

        [StringLength(200)]
        public string CompleteAddress { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public bool InsertReservationConfirmed { get; set; }

        [NotMapped]
        public double Distance { get; set; }

        [Column(TypeName = "geography")]
        public IPoint Location { get; set; }

        public virtual ICollection<CompanySchedule> CompanySchedules { get; set; }

        [ForeignKey("IdCity")]
        public virtual City City { get; set; }

    }
}
