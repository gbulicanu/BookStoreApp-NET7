namespace BookStore.Blazor.Server.UI.ApiServices
{
    public interface IAuthorService
    {
        Task<Response<List<AuthorDto>>> GetAuthors();
        Task<Response<AuthorDto>> GetAuthor(int id);
        Task<Response<AuthorUpdateDto>> GetAuthorForUpdate(int id);
        Task<Response<AuthorCreateDto>> CreateAuthor(AuthorCreateDto authorDto);
        Task<Response<int>> EditAuthor(int id, AuthorUpdateDto updateDto);
        Task<Response<int>> DeleteAuthor(int id);
    }
}