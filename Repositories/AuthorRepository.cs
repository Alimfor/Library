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

    public List<Author> GetAllAuthors()
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            return connection.Query<Author>("pGetAllOrOneAuthor", new { id = 0 },
                commandType: CommandType.StoredProcedure).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Author GetAuthorById(int id)
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            return (Author) connection.Query("pGetAllOrOneAuthor", new { id }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public int GetAuthorIdByFirstAndLastName(string firstName, string lastName)
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            int? authorId = connection.Query("pGetAuthorIdByFirstAndLastName", new { firstName, lastName}, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return authorId ?? 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Result SaveAuthor(Author author)
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            string result = 
                connection.ExecuteScalar<string>("pSaveAuthor",new {author.firstName,author.lastName},commandType: CommandType.StoredProcedure).ToUpper();
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

    public Result UpdateAuthor(Author author)
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            connection.Execute("pUpdateAuthor", new { author.authorId, author.firstName, author.lastName },
                commandType: CommandType.StoredProcedure);

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
                status = Status.WRONG_REQUEST
            };
        }
    }

    public Result DeleteAuthorById(int id)
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            connection.Execute("pDeleteBook", new { id }, commandType: CommandType.StoredProcedure);
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
    public List<ListSelect> AuthorSelect()
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
        return connection.Query<ListSelect>("pAuthorSelect", commandType: CommandType.StoredProcedure).ToList();
    }
}