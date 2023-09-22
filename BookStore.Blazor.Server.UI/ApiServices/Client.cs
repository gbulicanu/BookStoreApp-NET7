namespace BookStore.Blazor.Server.UI.ApiServices;

public partial class Client : IClient
{
    public HttpClient HttpClient
    {
        get { return _httpClient; }
    }
}

