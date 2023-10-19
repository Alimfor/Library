using library.Models;
using Library.Models;
using Library.Utils;

namespace Library.Repositories;

public interface IAuthorRepository
{
    OperationResult<IEnumerable<Author>> GetAllAuthors();
    OperationResult<Author> GetAuthorById(int id);
    OperationResult<int> GetAuthorIdByFirstAndLastName(string firstName, string lastName);
    OperationResult<IEnumerable<ListSelect>> AuthorSelect();
    Result SaveAuthor(Author author);
    Result UpdateAuthor(Author author);
    Result DeleteAuthorById(int id);
}