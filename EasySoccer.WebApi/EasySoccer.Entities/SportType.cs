using System.ComponentModel.DataAnnotations;

namespace EasySoccer.Entities
{
    public class SportType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }
    }
}
