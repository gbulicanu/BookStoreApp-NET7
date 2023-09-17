using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models.Users;

public class UserDto : LoginUserDto 
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    public string? Role { get; set; }
}