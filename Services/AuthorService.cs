using Library.Models;
using Library.Repositories;

namespace Library.Services;

public class AuthorService
{
    private readonly AuthorRepository _authorRepository;

    public AuthorService(AuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public Author GetAuthorById(int id)
    {
        return _authorRepository.GetAuthorById(id);
    }

    public int GetAuthorId(string firstName, string lastName)
    {
        return _authorRepository.GetAuthorIdByFirstAndLastName(firstName,lastName);
    }


}