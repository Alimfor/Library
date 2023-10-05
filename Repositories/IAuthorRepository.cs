using Library.Models;
using Library.Utils;

namespace Library.Repositories;

public interface IAuthorRepository
{
    Author GetAuthorById(int id);
    int GetAuthorIdByFirstAndLastName(string firstName, string lastName);
    Result SaveAuthor(Author author);
    Result UpdateAuthor(Author author);
    Result DeleteAuthor(Author author);

}