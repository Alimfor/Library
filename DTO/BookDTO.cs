using Library.Models;

namespace Library.DTO;

public class BookDTO
{
    public int bookId { get; set; }
    public string title { get; init; }
    public int year { get; init; }
    public AuthorDTO author { get; init; }
    public CategoryDTO category { get; init; }
}