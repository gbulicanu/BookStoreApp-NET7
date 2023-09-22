using System;
using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace BookStore.Blazor.Server.UI.ApiServices
{
	public class HttpServiceBase
	{
        private readonly IClient _client;
        private readonly ILocalStorageService _localStorage;

        public HttpServiceBase(IClient client, ILocalStorageService localStorage)
		{
            _client = client;
            _localStorage = localStorage;
        }

        protected Response<Guid> ConvertApiExceptions<Guid>(ApiException apiException)
        {
            if (apiException.StatusCode == 400)
            {
                return new Response<Guid>
                {
                    Message = "Validation errors occured",
                    ValidationErrors = apiException.Response,
                    Success = false
                };
            }
            if (apiException.StatusCode == 404)
            {
                return new Response<Guid>
                {
                    Message = "The requested item could not be found",
                    ValidationErrors = apiException.Response,
                    Success = false
                };
            }

            return new Response<Guid>
            {
                Message = "Something went wrong, please try again.",
                ValidationErrors = apiException.Response,
                Success = false
            };
        }

        protected async Task GetBearerToken()
        {
            var token = await _localStorage.GetItemAsync<string>("accessToken");
            if (token is not null)
            {
                _client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        } 
	}
}

