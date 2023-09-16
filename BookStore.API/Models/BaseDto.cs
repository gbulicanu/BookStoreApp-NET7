using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models;

public class BaseDto
{
    [Required]
    public int Id { get; set; }
}