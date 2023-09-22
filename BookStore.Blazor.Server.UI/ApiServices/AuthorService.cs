using AutoMapper;
using Blazored.LocalStorage;

namespace BookStore.Blazor.Server.UI.ApiServices;

public class AuthorService : HttpServiceBase, IAuthorService
{
    private readonly IClient _client;
    private readonly IMapper _mapper;

    public AuthorService(
        IClient client,
        ILocalStorageService localStorage,
        IMapper mapper)
        : base(client, localStorage)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task<Response<List<AuthorDto>>> GetAuthors()
    {
        Response<List<AuthorDto>> response;

        try
        {
            await GetBearerToken();
            var data = await _client.AuthorsAllAsync();
            response = new Response<List<AuthorDto>>
            {
                Data = data.ToList(),
                Success = true,
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiExceptions<List<AuthorDto>>(ex);
        }

        return response;
    }

    public async Task<Response<AuthorDto>> GetAuthor(int id)
    {
        Response<AuthorDto> response;

        try
        {
            await GetBearerToken();
            var data = await _client.AuthorsGETAsync(id);
            response = new Response<AuthorDto>
            {
                Data = data,
                Success = true,
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiExceptions<AuthorDto>(ex);
        }

        return response;
    }

    public async Task<Response<AuthorUpdateDto>> GetAuthorForUpdate(int id)
    {
        Response<AuthorUpdateDto> response;

        try
        {
            await GetBearerToken();
            var data = await _client.AuthorsGETAsync(id);
            response = new Response<AuthorUpdateDto>
            {
                Data = MapToUpdate(data),
                Success = true,
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiExceptions<AuthorUpdateDto>(ex);
        }

        return response;
    }

    public async Task<Response<AuthorCreateDto>> CreateAuthor(AuthorCreateDto authorDto)
	{
        Response<AuthorCreateDto> response;

        try
        {
            await GetBearerToken();
            var data = await _client.AuthorsPOSTAsync(authorDto);
            response = new Response<AuthorCreateDto>
            {
                Data = data,
                Success = true,
            };
        }
        catch (ApiException ex)
        {
            response = ConvertApiExceptions<AuthorCreateDto>(ex);
        }

        return response;
    }

    public async Task<Response<int>> EditAuthor(int id, AuthorUpdateDto authorDto)
    {
        Response<int> response = new();

        try
        {
            await GetBearerToken();
            await _client.AuthorsPUTAsync(id, authorDto);
        }
        catch (ApiException ex)
        {
            response = ConvertApiExceptions<int>(ex);
        }

        return response;
    }

    public async Task<Response<int>> DeleteAuthor(int id)
    {
        Response<int> response;

        try
        {
            await GetBearerToken();
            await _client.AuthorsDELETEAsync(id);
            response = new Response<int>() { Success = true };
        }
        catch (ApiException ex)
        {
            response = ConvertApiExceptions<int>(ex);
        }

        return response;
    }

    private AuthorUpdateDto MapToUpdate(AuthorDto author)
    {
        return _mapper.Map<AuthorUpdateDto>(author);
    }
}

