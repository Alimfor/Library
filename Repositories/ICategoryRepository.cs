using library.Models;
using Library.Models;

namespace Library.Repositories;

public interface ICategoryRepository
{
    Category GetCategoryById(int id);
    List<ListSelect> CategorySelect();
}