using System;
using System.ComponentModel.DataAnnotations;

namespace EasySoccer.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string SocialMediaId { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [StringLength(200)]
        public string Password { get; set; }
    }
}
