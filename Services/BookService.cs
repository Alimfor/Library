using Library.Models;
using Library.Repositories;
using Library.Utils;

namespace Library.Services
{
    public class BookService
    {
        private readonly BookRepository _bookRepository;

        public BookService(BookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public OperationResult<IEnumerable<Book>> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }

        public OperationResult<Book> GetBookById(int id)
        {
            return _bookRepository.GetBookById(id);
        }
        
        public Result SaveBook(Book book)
        {
            return _bookRepository.SaveBook(book);
        }

        public Result UpdateBook(Book book, int bookId)
        {
            book.bookId = bookId;
            return _bookRepository.UpdateBook(book);
        }

        public Result DeleteBook(string bookName, string authorName, string categoryName)
        {
             var bookId = GetBookId(bookName, authorName, categoryName);
             return _bookRepository.DeleteBookById(bookId.data);
        }
        
        private OperationResult<int> GetBookId(string bookName, string authorName, string categoryName)
        {
            return _bookRepository.GetBookIdByBookDetails(bookName, authorName, categoryName);
        }

        public Result DeleteBook(int bookId)
        {
            return _bookRepository.DeleteBookById(bookId);
        }
    }
}
