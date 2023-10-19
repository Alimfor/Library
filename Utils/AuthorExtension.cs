using Library.DTO;
using Library.Models;

namespace Library.Utils;

public static class AuthorExtension
{
    public static AuthorDTO ToAuthorDto(this Author author)
    {
        return new AuthorDTO()
        {
            firstName = author.firstName,
            lastName = author.lastName
        };
    }

    public static Author ToAuthor(this AuthorDTO authorDto)
    {
        return new Author()
        {
            firstName = authorDto.firstName,
            lastName = authorDto.lastName
        };
    }
}