using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models.Authors;

public class AuthorUpdateDto: BaseDto
{
    [Required]
    [StringLength(50)]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public required string LastName { get; set; }

    [StringLength(250)]
    public string? Bio { get; set; }
}

