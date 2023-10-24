using Dapper.FluentMap.Mapping;
using Library.Models;

namespace Library.Utils;

public class CategoryMap : EntityMap<Category>
{
    public CategoryMap()
    {
        Map(category => category.categoryId).ToColumn("category_id");
    }
}