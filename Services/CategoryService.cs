using library.Models;
using Library.Models;
using Library.Repositories;
using Library.Utils;

namespace Library.Services;

public class CategoryService
{
    private readonly CategoryRepository _categoryRepository;

    public CategoryService(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public OperationResult<IEnumerable<Category>> GetAllCategories()
    {
        return _categoryRepository.GetAllCategories();
    }
    
    public OperationResult<Category> GetCategoryById(int id)
    {
        return _categoryRepository.GetCategoryById(id);
    }

    public OperationResult<int> GetCategoryIdByName(string categoryName)
    {
        return _categoryRepository.GetCategoryIdByName(categoryName);
    }

    public OperationResult<IEnumerable<ListSelect>> CategorySelect()
    {
        return _categoryRepository.CategorySelect();
    }
    
    public Result SaveCategory(Category newCategory)
    {
        return _categoryRepository.SaveCategory(newCategory);
    }

    public Result UpdateCategory(Category updatedCategory, int categoryId)
    {
        updatedCategory.categoryId = categoryId;
        return _categoryRepository.UpdateCategory(updatedCategory);
    }

    public Result DeleteCategoryById(int id)
    {
        return _categoryRepository.DeleteCategoryById(id);
    }
}