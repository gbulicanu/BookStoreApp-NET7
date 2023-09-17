namespace BookStore.API.Models.Users;

public class AuthResponse
{
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Email { get; set; } = null!;
}

