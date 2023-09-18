namespace BookStore.Blazor.Server.UI.ApiServices;

public interface IAuthClient
{
    Task<bool> AuthenticateAsync(LoginUserDto loginModel);
    Task LogoutAsync();
}