using System.ComponentModel.DataAnnotations;

namespace Consumer1;

public class UserCreatedEvent
{
    [Required]
    [Range(0, 10)]
    public int? Id { get; set; }
}
