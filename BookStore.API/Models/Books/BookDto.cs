using System;
namespace BookStore.API.Models.Books;

public class BookDto : BaseDto
{
    public string? Title { get; set; }

    public int? Year { get; set; }

    public string Isbn { get; set; } = null!;

    public string? Image { get; set; }

    public decimal? Price { get; set; }

    public int AuthorId { get; set; }

    public string? AuthorName { get; set; }
}
