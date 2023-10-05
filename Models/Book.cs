namespace Library.Models
{
    public class Book
    {
        public int bookId { get; set; }
        public string title { get; set; }
        public int year { get; set; }
        public int authorId { get; set; }
        public int categoryId { get; set; }
    }
}
