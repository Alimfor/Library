using Library.Models;

namespace Library.Repositories;

public interface ICategoryRepository
{
    Category GetCategoryById(int id);
}