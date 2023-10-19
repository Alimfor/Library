using Library.DTO;
using Library.Models;

namespace Library.Utils;

public static class CategoryExtension
{
    public static CategoryDTO ToCategoryDto(this Category category)
    {
        return new CategoryDTO()
        {
            name = category.name
        };
    }

    public static Category ToCategory(this CategoryDTO categoryDto)
    {
        return new Category()
        {
            name = categoryDto.name
        };
    }
}