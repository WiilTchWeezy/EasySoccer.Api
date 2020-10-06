using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySoccer.Entities
{
    public class State
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int IBGECode { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
