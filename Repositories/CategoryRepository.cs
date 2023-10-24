using System.Data;
using System.Data.SqlClient;
using Dapper;
using library.Models;
using Library.Models;
using Library.Utils;

namespace Library.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IConfiguration _configuration;

    public CategoryRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public OperationResult<IEnumerable<Category>> GetAllCategories()
        {
            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
                var categories = connection.Query<Category>("pGetAllOrOneCategory", new { id = 0 },
                    commandType: CommandType.StoredProcedure);
    
                return new OperationResult<IEnumerable<Category>>
                {
                    data = categories,
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
                return new OperationResult<IEnumerable<Category>>
                {
                    data = Enumerable.Empty<Category>(),
                    result = new Result
                    {
                        code = 500,
                        message = ex.Message,
                        status = Status.ERROR
                    }
                };
            }
        }
    
    public OperationResult<Category> GetCategoryById(int id)
             {
                 try
                 {
                     using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
                     var category =  connection.Query<Category>("pGetAllOrOneCategory", new { id }, commandType: CommandType.StoredProcedure)
                         .FirstOrDefault();
                     
                     if (category != null)
                     {
                         return new OperationResult<Category>
                         {
                             data = category,
                             result = new Result
                             {
                                 code = 200,
                                 message = "done",
                                 status = Status.SUCCESSFUL
                             }
                         };
                     }
                             
                     return new OperationResult<Category>
                     {
                         data = new Category(),
                         result = new Result
                         {
                             code = 400,
                             message = "the category object, did not has any dates",
                             status = Status.WRONG_REQUEST
                         }
                     };
                 }
                 catch (Exception ex)
                 {
                     return new OperationResult<Category>
                     {
                         data = new Category(),
                         result = new Result
                         {
                             code = 500,
                             message = ex.Message,
                             status = Status.ERROR
                         }
                     };
                 }
             }

    public OperationResult<int> GetCategoryIdByName(string categoryName)
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            int? categoryId = connection.Query("pGetCategoryIdByName", new { categoryName }, 
                commandType: CommandType.StoredProcedure).FirstOrDefault();
            
            if (categoryId != null)
            {
                return new OperationResult<int>()
                {
                    data = (int) categoryId,
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
    
    public OperationResult<IEnumerable<ListSelect>> CategorySelect()
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            var categorySelects = connection.Query<ListSelect>("pCategorySelect",
                commandType: CommandType.StoredProcedure).ToList();
            
            return new OperationResult<IEnumerable<ListSelect>>
            {
                data = categorySelects,
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
    
    public Result SaveCategory(Category category)
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            var result =
                connection.ExecuteScalar<string>("pSaveCategory", new { category.name }, 
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

    public Result UpdateCategory(Category category)
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            var changedRows = connection.Execute("pUpdateCategory", new { id = category.categoryId, category.name },
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

    public Result DeleteCategoryById(int id)
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("conStr"));
            var changedRows = connection.Execute("pDeleteCategory", new { id }, commandType: CommandType.StoredProcedure);
            
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