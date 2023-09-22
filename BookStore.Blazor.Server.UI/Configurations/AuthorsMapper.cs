using AutoMapper;
using BookStore.Blazor.Server.UI.ApiServices;

namespace BookStore.Blazor.Server.UI.Configurations;

public class AuthorsMapper : Profile
{
    public AuthorsMapper()
    {
        CreateMap<AuthorDto, AuthorUpdateDto>()
            .ReverseMap();
    }
}