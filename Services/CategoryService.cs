using library.Models;
using Library.Models;
using Library.Repositories;

namespace Library.Services;

public class CategoryService
{
    private readonly CategoryRepository _categoryRepository;

    public CategoryService(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Category GetCategoryById(int id)
    {
        return _categoryRepository.GetCategoryById(id);
    }

    public int GetCategoryIdByName(string categoryName)
    {
        return 0;
    }

    public List<ListSelect> CategorySelect()
    {
        return _categoryRepository.CategorySelect();
    }
}