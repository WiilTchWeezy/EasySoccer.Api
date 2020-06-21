using EasySoccer.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EasySoccer.Entities
{
    public class FormInput
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(800)]
        public string InputData { get; set; }

        [Required]
        public FormTypeEnum FormType { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public string Message { get; set; }

        [Required]
        public FormStatusEnum Status { get; set; }
    }
}
