using System.Data;
using System.Data.SqlClient;
using Dapper;
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

    public Result DeleteAuthor(Author author)
    {
        throw new NotImplementedException();
    }

    public Author GetAuthorById(int id)
    {
        try
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("conStr")))
            {
                return (Author) connection.Query("pGetAllOrOneAuthor", new { id }, commandType: CommandType.StoredProcedure);
            }
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
            using (var connection = new SqlConnection(_configuration.GetConnectionString("conStr")))
            {
                int? authorId = connection.Query("pGetAuthorIdByFirstAndLastName", new { firstName, lastName}, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return authorId ?? 0;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Result SaveAuthor(Author author)
    {
        throw new NotImplementedException();
    }

    public Result UpdateAuthor(Author author)
    {
        throw new NotImplementedException();
    }
}