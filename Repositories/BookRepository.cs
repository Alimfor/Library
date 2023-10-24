using Library.Models;
using Library.Utils;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Library.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IConfiguration _configuration;

        public BookRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public OperationResult<IEnumerable<Book>> GetAllBooks()
        {
            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
                var bookList = connection.Query<Book>("pGetAllOrOneBook", new {id = 0}, 
                    commandType: CommandType.StoredProcedure).ToList();

                return new OperationResult<IEnumerable<Book>>
                {
                    data = bookList,
                    result = new Result
                    {
                        code = 200,
                        message = "done",
                        status = Status.SUCCESSFUL
                    }
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<IEnumerable<Book>>
                {
                    data = Enumerable.Empty<Book>(),
                    result = new Result
                    {
                        code = 500,
                        message = ex.Message,
                        status = Status.ERROR
                    }
                };
            }
        }

        public OperationResult<Book> GetBookById(int id)
        {
            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
                var book = connection.Query<Book>("pGetAllOrOneBook",new { id }, commandType: CommandType.StoredProcedure)
                    .FirstOrDefault();

                if (book != null)
                {
                    return new OperationResult<Book>
                    {
                        data = book,
                        result = new Result
                        {
                            code = 200,
                            message = "done",
                            status = Status.SUCCESSFUL
                        }
                    };
                }
                    
                return new OperationResult<Book>
                {
                    data = new Book(),
                    result = new Result
                    {
                        code = 400,
                        message = "the book object, did not has any dates",
                        status = Status.WRONG_REQUEST
                    }
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<Book>
                {
                    data = new Book(),
                    result = new Result
                    {
                        code = 500,
                        message = ex.Message,
                        status = Status.ERROR
                    }
                };
            }
        }

        public OperationResult<int> GetBookIdByBookDetails(string bookName, string authorName, string categoryName)
        {
            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
                int? bookId = connection.Query<int>("pGetBookId", new { bookName, authorName, categoryName },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (bookId != null)
                {
                    return new OperationResult<int>()
                    {
                        data = (int)bookId,
                        result = new Result()
                        {
                            message = "done",
                            code = 200,
                            status = Status.SUCCESSFUL
                        }
                    };

                }

                return new OperationResult<int>()
                {
                    data = -1,
                    result = new Result()
                    {
                        message = "wrong request",
                        code = 400,
                        status = Status.WRONG_REQUEST
                    }
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<int>
                {
                    data = -1,
                    result = new Result
                    {
                        code = 500,
                        message = ex.Message,
                        status = Status.ERROR
                    }
                };
            }
        }

        public Result SaveBook(Book book)
        {
            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
                var result = 
                    connection.ExecuteScalar<string>("pSaveBook", new
                        {
                            book.title, book.year, book.authorId, catalogId = book.categoryId
                        },
                        commandType: CommandType.StoredProcedure);
                
                if (!string.IsNullOrEmpty(result) && 
                    string.Equals(result, Status.SUCCESSFUL.ToString(), 
                        StringComparison.OrdinalIgnoreCase))
                {
                    return new Result
                    {
                        message = result,
                        code = 200,
                        status = Status.SUCCESSFUL
                    };
                }

                return new Result
                {
                    message = "one, either all dates is wrong",
                    code = 400,
                    status = Status.WRONG_REQUEST
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    message = ex.Message,
                    code = 500,
                    status = Status.ERROR
                };
            }
        }

        public Result UpdateBook(Book book)
        {
            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
                var changedRows = connection.Execute("pUpdateBook", new { 
                    id = book.bookId,book.title, book.year, book.authorId, catalogId = book.categoryId 
                }, commandType: CommandType.StoredProcedure);
                
                if (changedRows != 0)
                {
                    return new Result
                    {
                        message = "done",
                        code = 200,
                        status = Status.SUCCESSFUL
                    };
                }
                
                return new Result
                {
                    message = "no one row did not changed",
                    code = 400,
                    status = Status.WRONG_REQUEST
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    message = ex.Message,
                    code = 500,
                    status = Status.ERROR
                };
            }
        }

        public Result DeleteBookById(int id)
        {
            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
                var changedRows =
                    connection.Execute("pDeleteBook", new { id }, commandType: CommandType.StoredProcedure);

                if (changedRows != 0)
                {
                    return new Result
                    {
                        message = "done",
                        code = 200,
                        status = Status.SUCCESSFUL
                    };
                }

                return new Result
                {
                    message = "no one row did not changed",
                    code = 400,
                    status = Status.WRONG_REQUEST
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    message = ex.Message,
                    code = 500,
                    status = Status.ERROR
                };
            }
        }
    }
}
