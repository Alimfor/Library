using Library.Models;

namespace Library.DTO;

public class BookDTO
{
    public string title { get; set; }
    public int year { get; set; }
    public AuthorDTO author { get; set; }
    public CategoryDTO category { get; set; }
}