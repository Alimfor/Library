using Dapper.FluentMap.Mapping;
using Library.Models;

namespace Library.Utils;

public class AuthorMap : EntityMap<Author>
{
    public AuthorMap()
    {
        Map(author => author.authorId).ToColumn("author_id");
        Map(author => author.firstName).ToColumn("first_name");
        Map(author => author.lastName).ToColumn("last_name");
    }
}