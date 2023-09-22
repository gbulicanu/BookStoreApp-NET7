namespace BookStore.Blazor.Server.UI.ApiServices;

public class Response<T>
{
	public string Message { get; set; } = null!;
	public string ValidationErrors { get; set; } = null!;
	public bool Success { get; set; }
	public T Data { get; set; } = default!;
}

