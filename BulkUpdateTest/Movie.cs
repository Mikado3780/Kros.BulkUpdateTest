using Kros.KORM.Metadata.Attribute;

namespace BulkUpdateTest
{
    [Alias("Movies")]
    public class Movie
    {
        [Key]
        [Alias("Id")]
        public int Id { get; set; }

        [Alias("Name")]
        public string Name { get; set; }

        [Alias("Year")]
        public int? Year { get; set; }
    }
}
