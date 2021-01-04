﻿using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasySoccer.Entities
{
    public class CompanyUserNotification
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public NotificationTypeEnum NotificationTypeEnum { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(500)]
        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public bool Read { get; set; }

        [Required]
        public long IdCompanyUser { get; set; }
    }
}
