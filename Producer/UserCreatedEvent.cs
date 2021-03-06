using System.ComponentModel.DataAnnotations;

namespace Producer;

public class UserCreatedEvent
{
    [Required]
    [Range(0, 10)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0, 10)]
    public decimal Price { get; set; }
}
