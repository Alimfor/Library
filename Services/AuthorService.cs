using Library.Models;
using Library.Repositories;
using Library.Utils;

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

    public List<Author> GetAllAuthors()
    {
        return _authorRepository.GetAllAuthors();
    }

    public Result SaveAuthor(Author newAuthor)
    {
        return _authorRepository.SaveAuthor(newAuthor);
    }


    public Result UpdateAuthor(Author updatedAuthor)
    {
        return _authorRepository.UpdateAuthor(updatedAuthor);
    }

    public Result DeleteAuthorById(int id)
    {
        return _authorRepository.DeleteAuthorById(id);
    }
}