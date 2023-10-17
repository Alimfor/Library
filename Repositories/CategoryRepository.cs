using System.Data;
using System.Data.SqlClient;
using Dapper;
using library.Models;
using Library.Models;

namespace Library.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IConfiguration _configuration;

    public CategoryRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public Category GetCategoryById(int id)
    {
        try
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("conStr")))
            {
                return (Category) connection.Query("pGetAllOrOneCategory", new { id }, commandType: CommandType.StoredProcedure);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public List<ListSelect> CategorySelect()
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
        return connection.Query<ListSelect>("pCategorySelect", commandType: CommandType.StoredProcedure).ToList();
    }
}