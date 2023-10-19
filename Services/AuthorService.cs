using library.Models;
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
    
    public OperationResult<IEnumerable<Author>> GetAllAuthors()
    {
        return _authorRepository.GetAllAuthors();
    }
    
    public OperationResult<Author> GetAuthorById(int id)
    {
        return _authorRepository.GetAuthorById(id);
    }

    public OperationResult<int> GetAuthorId(string firstName, string lastName)
    {
        return _authorRepository.GetAuthorIdByFirstAndLastName(firstName,lastName);
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

    public OperationResult<IEnumerable<ListSelect>> AuthorSelect()
    {
        return _authorRepository.AuthorSelect();
    }
}