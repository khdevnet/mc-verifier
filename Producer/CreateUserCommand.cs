using System.ComponentModel.DataAnnotations;

namespace Producer
{
    public class CreateUserCommand
    {
        [Required]
        [Range(0, 10)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, 10)]
        public decimal Price { get; set; }
    }
}