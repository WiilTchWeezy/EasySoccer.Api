using EasySoccer.Entities.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace EasySoccer.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        public string SocialMediaId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [StringLength(200)]
        [Required]
        public string Password { get; set; }

        [Required]
        public CreatedFromEnum CreatedFrom { get; set; }

        public string PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiresDate { get; set; }
    }
}
