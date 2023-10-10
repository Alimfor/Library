using Library.Models;
using Library.Utils;

namespace Library.Repositories
{
    public interface IBookRepository
    {
        List<Book> GetAllBooks();
        Book GetBookById(int id);
        Result SaveBook(Book book);
        Result UpdateBook(Book book);
        Result DeleteBook(int id);
        int GetBookIdByBookDetails(string bookName, string authorName, string categoryName);
    }
}
