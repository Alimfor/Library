using Dapper.FluentMap.Mapping;
using Library.Models;

namespace Library.Utils;

public class BookMap : EntityMap<Book>
{
    public BookMap()
    {
        Map(book => book.bookId).ToColumn("book_id");
        Map(book => book.authorId).ToColumn("author_id");
        Map(book => book.categoryId).ToColumn("category_id");
    }
}