using AutoMapper;
using BookStore.API.Data;
using BookStore.API.Models.Books;

namespace BookStore.API.Configurations;

public class BooksMapper : Profile
{
    public BooksMapper()
    {
        Func<Book, string> extractAuthorName = (book) => {
            if (book != null && book.Author != null)
                return $"{book.Author.FirstName} {book.Author.LastName}";

            return null!; 
        };

        CreateMap<BookCreateDto, Book>()
            .ReverseMap();
        CreateMap<BookDto, Book>()
            .ReverseMap();
        CreateMap<BookUpdateDto, Book>()
            .ReverseMap();
        CreateMap<Book, BookDto>()
            .ForMember(
                x => x.AuthorName,
                y => y.MapFrom(z => extractAuthorName(z)))
            .ReverseMap();
    }
}