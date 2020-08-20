using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EasySoccer.Entities
{
    public class Person
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(300)]
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public CreatedFromEnum CreatedFrom { get; set; }

        public Guid? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
