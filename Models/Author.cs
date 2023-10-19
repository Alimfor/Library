using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    [Table("author")]
    public class Author
    {
        [Column("author_id")]
        public int authorId { get; set; }

        [Column("first_name")]
        public string firstName { get; set; }

        [Column("last_name")]
        public string lastName { get; set; }
    }
}
