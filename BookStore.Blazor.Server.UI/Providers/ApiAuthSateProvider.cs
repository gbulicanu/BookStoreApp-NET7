using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStore.Blazor.Server.UI.Providers;

public class ApiAuthSateProvider : AuthenticationStateProvider
	{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly ILocalStorageService _localStorage;

    public ApiAuthSateProvider(ILocalStorageService localStorage)
		{
        _localStorage = localStorage;
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var savedToken = await _localStorage.GetItemAsync<string>("accessToken");
        if (savedToken == null)
        {
            return new AuthenticationState(user);
        }

        var tokenContent = _tokenHandler.ReadJwtToken(savedToken);
        if (tokenContent.ValidTo.ToLocalTime() < DateTime.Now)
        {
            return new AuthenticationState(user);
        }

        user = new ClaimsPrincipal(new ClaimsIdentity(await GetClaims(), "jwt"));
        return new AuthenticationState(user);
    }

    public async Task LoggedIn()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(await GetClaims(), "jwt"));
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }

    public async Task LoggedOut()
    {
        await _localStorage.RemoveItemAsync("accessToken");
        var nobody = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(nobody));
        NotifyAuthenticationStateChanged(authState);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var savedToken = await _localStorage.GetItemAsync<string>("accessToken");
        var tokenContent = _tokenHandler.ReadJwtToken(savedToken);
        var claims = tokenContent.Claims.ToList();
        claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
        return claims;
    }
}

