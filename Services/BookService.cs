using Library.Models;
using Library.Repositories;
using Library.Utils;

namespace Library.Services
{
    public class BookService
    {
        private BookRepository _bookRepository;

        public BookService(BookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }

        public Book GetBookById(int id)
        {
            return _bookRepository.GetBookById(id);
        }
        
        public Result SaveBook(Book book)
        {
            return _bookRepository.SaveBook(book);
        }

        public Result UpdateBook(Book book)
        {
            return _bookRepository.UpdateBook(book);
        }

        public Result DeleteBook(string bookName, string authorName, string categoryName)
        {
             int bookId = GetBookId(bookName, authorName, categoryName);
             return _bookRepository.DeleteBook(bookId);
        }
        
        private int GetBookId(string bookName, string authorName, string categoryName)
        {
            return _bookRepository.GetBookIdByBookDetails(bookName, authorName, categoryName);
        }
    }
}
