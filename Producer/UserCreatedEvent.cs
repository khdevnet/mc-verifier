using System.ComponentModel.DataAnnotations;

namespace Events.Producer
{
    public class UserCreatedEvent
    {
        [Required]
        [Range(0, 10)]
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        [Range(0, 10)]
        public decimal Price { get; set; }
    }
}