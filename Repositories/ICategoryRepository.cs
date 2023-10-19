using library.Models;
using Library.Models;
using Library.Utils;

namespace Library.Repositories;

public interface ICategoryRepository
{
    OperationResult<IEnumerable<Category>> GetAllCategories();
    OperationResult<Category> GetCategoryById(int id);
    OperationResult<int> GetCategoryIdByName(string categoryName);
    OperationResult<IEnumerable<ListSelect>> CategorySelect();
    Result SaveCategory(Category category);
    Result UpdateCategory(Category category);
    Result DeleteCategoryById(int id);
}
