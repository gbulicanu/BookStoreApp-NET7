using AutoMapper;
using BookStore.API.Data;
using BookStore.API.Models.Authors;

namespace BookStore.API.Configurations;

public class AuthorsMapper: Profile
{
    public AuthorsMapper()
    {
        CreateMap<AuthorCreateDto, Author>()
            .ReverseMap();
        CreateMap<AuthorDto, Author>()
            .ReverseMap();
        CreateMap<AuthorUpdateDto, Author>()
            .ReverseMap();
    }
} 

