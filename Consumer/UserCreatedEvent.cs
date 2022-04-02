using System.ComponentModel.DataAnnotations;

namespace Consumer
{
    public class UserCreatedEvent
    {
        [Required]
        [Range(0, 10)]
        public int? Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }
    }
}