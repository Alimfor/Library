using Library.Models;
using Library.Utils;

namespace Library.Repositories
{
    public interface IBookRepository
    {
        OperationResult<IEnumerable<Book>> GetAllBooks();
        OperationResult<Book> GetBookById(int id);
        OperationResult<int> GetBookIdByBookDetails(string bookName, string authorName, string categoryName);
        Result SaveBook(Book book);
        Result UpdateBook(Book book);
        Result DeleteBookById(int id);
    }
}
