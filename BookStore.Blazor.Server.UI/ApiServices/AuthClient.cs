using Blazored.LocalStorage;
using BookStore.Blazor.Server.UI.Providers;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStore.Blazor.Server.UI.ApiServices;

public class AuthClient : IAuthClient
{
    private readonly IClient _client;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthClient(
        IClient client,
        ILocalStorageService localStorage,
        AuthenticationStateProvider authSateProvider)
    {
        _client = client;
        _localStorage = localStorage;
        _authStateProvider = authSateProvider;
    }

    public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
    {
        var response = await _client.LoginAsync(loginModel);

        // Store Token
        await _localStorage.SetItemAsync("accessToken", response.Token);

        // Change Auth state of the app
        await ((ApiAuthSateProvider)_authStateProvider).LoggedIn();

        return true;
    }
}

