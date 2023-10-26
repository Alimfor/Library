using System.Data;
using System.Data.SqlClient;
using Dapper;
using library.Models;
using Library.Models;
using Library.Utils;

namespace Library.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly IConfiguration _configuration;

    public AuthorRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public OperationResult<IEnumerable<Author>> GetAllAuthors()
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            var authors = connection.Query<Author>("pGetAllOrOneAuthor", new { id = 0 },
                commandType: CommandType.StoredProcedure);

                return new OperationResult<IEnumerable<Author>>
                {
                    data = authors,
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
            return new OperationResult<IEnumerable<Author>>
            {
                data = Enumerable.Empty<Author>(),
                result = new Result
                {
                    code = 500,
                    message = ex.Message,
                    status = Status.ERROR
                }
            };
        }
    }

    public OperationResult<Author> GetAuthorById(int id)
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            var author = connection.Query<Author>("pGetAllOrOneAuthor", new { id }, commandType: CommandType.StoredProcedure)
                .FirstOrDefault();
            
            if (author != null)
            {
                return new OperationResult<Author>
                {
                    data = author,
                    result = new Result
                    {
                        code = 200,
                        message = "done",
                        status = Status.SUCCESSFUL
                    }
                };
            }
                    
            return new OperationResult<Author>
            {
                data = new Author(),
                result = new Result
                {
                    code = 400,
                    message = "the author object, did not has any dates",
                    status = Status.WRONG_REQUEST
                }
            };
        }
        catch (Exception ex)
        {
            return new OperationResult<Author>
            {
                data = new Author(),
                result = new Result
                {
                    code = 500,
                    message = ex.Message,
                    status = Status.ERROR
                }
            };
        }
    }

    public OperationResult<int> GetAuthorIdByFirstAndLastName(string firstName, string lastName)
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            int authorId = connection.Query<int>("pGetAuthorIdByFirstAndLastName", new { firstName, lastName},
                commandType: CommandType.StoredProcedure).FirstOrDefault();
            
            if (authorId == -1)
            {
                return new OperationResult<int>()
                {
                    data = authorId,
                    result = new Result()
                    {
                        message = "wrong request",
                        code = 400,
                        status = Status.WRONG_REQUEST
                    }
                };

            }

            return new OperationResult<int>()
            {
                data = authorId,
                result = new Result()
                {
                    message = "done",
                    code = 200,
                    status = Status.SUCCESSFUL
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
   
    public OperationResult<IEnumerable<ListSelect>> AuthorSelect()
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            var authorSelects = connection.Query<ListSelect>("pAuthorSelect", 
                commandType: CommandType.StoredProcedure);
            
            return new OperationResult<IEnumerable<ListSelect>>
            {
                data = authorSelects,
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
            return new OperationResult<IEnumerable<ListSelect>>
            {
                data = Enumerable.Empty<ListSelect>(),
                result = new Result
                {
                    code = 500,
                    message = ex.Message,
                    status = Status.ERROR
                }
            };
        }
    }
    
    public Result SaveAuthor(Author author)
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            var result = 
                connection.ExecuteScalar<string>("pSaveAuthor",new {author.firstName,author.lastName}, 
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
                code = 500,
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

    public Result UpdateAuthor(Author author)
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            var changedRows = connection.Execute("pUpdateAuthor", new { id = author.authorId, author.firstName, author.lastName },
                commandType: CommandType.StoredProcedure);

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

    public Result DeleteAuthorById(int id)
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            var changedRows = connection.Execute("pDeleteAuthor", new { id }, commandType: CommandType.StoredProcedure);
            
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