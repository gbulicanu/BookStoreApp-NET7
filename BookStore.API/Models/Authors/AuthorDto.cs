using System;
namespace BookStore.API.Models.Authors;

public class AuthorDto : BaseDto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Bio { get; set; }
}
