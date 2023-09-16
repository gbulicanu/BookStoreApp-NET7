using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models.Books;

public class BookUpdateDto: BaseDto
{
    [Required]
    [StringLength(50)]
    public string? Title { get; set; }

    [Required]
    public int? Year { get; set; }

    [Required]
    public string Isbn { get; set; } = null!;

    [StringLength(250)]
    public string? Summary { get; set; }

    [StringLength(50)]
    public string? Image { get; set; }

    [Required]
    public decimal? Price { get; set; }

    [Required]
    public int? AuthorId { get; set; }
}

