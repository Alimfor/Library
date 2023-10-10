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

        public Result DeleteBook(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("conStr")))
                {
                    connection.Execute("pDeleteBook", new { id }, commandType: CommandType.StoredProcedure);
                }
                return new Result
                {
                    error = "none",
                    code = 200,
                    status = Status.SUCCESSFUL
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    error = ex.Message,
                    code = 500,
                    status = Status.SUCCESSFUL
                };
            }
        }

        public int GetBookIdByBookDetails(string bookName, string authorName, string categoryName)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("conStr")))
                {
                    int? bookId = connection.Query<int>("pGetBookId",new { bookName,authorName,categoryName }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (bookId == null)
                    {
                        const int UNDEFINED_ID = 0;
                        return UNDEFINED_ID;
                    }
                    return (int)bookId;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Book> GetAllBooks()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("conStr")))
                {
                    return connection.Query<Book>("pGetAllOrOneBook", new {id = 0}, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Book GetBookById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("conStr")))
                {
                    return connection.Query<Book>("pGetAllOrOneBook",new { id }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Result SaveBook(Book book)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("conStr")))
                {
                    string result = 
                        connection.ExecuteScalar<string>("pSaveBook", new { book.title, book.year, book.authorId, catalogId = book.categoryId }, commandType: CommandType.StoredProcedure)
                            .ToUpper();
                    if (result.Equals(Status.SUCCESSFUL))
                    {
                        return new Result
                        {
                            error = "none",
                            code = 200,
                            status = Status.SUCCESSFUL
                        };
                    }

                    return new Result
                    {
                        error = result,
                        code = 500,
                        status = Status.WRONG_REQUEST
                    };
                }
            }
            catch (Exception ex)
            {
                return new Result
                {
                    error = ex.Message,
                    code = 500,
                    status = Status.WRONG_REQUEST
                };
            }
        }

        public Result UpdateBook(Book book)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("conStr")))
                {
                    connection.Execute("pUpdateBook", new { book.bookId,book.title, book.year, book.authorId,
                        catalogId = book.categoryId }, commandType: CommandType.StoredProcedure);
                    return new Result
                    {
                        error = "none",
                        code = 200,
                        status = Status.SUCCESSFUL
                    };
                }
            }
            catch (Exception ex)
            {
                return new Result
                {
                    error = ex.Message,
                    code = 500,
                    status = Status.WRONG_REQUEST
                };
            }
        }
    }
}
