using System.ComponentModel.DataAnnotations;

namespace softsolutions.Models
{
    public class Blogs
    {
        [Key]
        public int BlogId { get; set; }

        [Required]
        public string BlogTitle { get; set; } = string.Empty;

        [Required]
        public string BlogContent { get; set; } = string.Empty;

        [Required]
        public string BlogAuthor { get; set; } = string.Empty;

    }
}
