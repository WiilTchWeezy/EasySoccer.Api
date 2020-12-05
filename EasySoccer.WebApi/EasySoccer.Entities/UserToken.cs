using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasySoccer.Entities
{
    public class UserToken
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public TokenTypeEnum TokenType { get; set; }
        [Required]
        [StringLength(300)]
        public string Token { get; set; }
        public Guid? UserId { get; set; }
        public long? CompanyUserId { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime? LogOffDate { get; set; }
    }
}
