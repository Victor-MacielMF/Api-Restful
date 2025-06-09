using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment
{
    public class CommentBaseDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Title must be at least 3 character long.")]
        [MaxLength(150, ErrorMessage = "Title must not exceed 150 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Content must be at least 5 character long.")]
        [MaxLength(1000, ErrorMessage = "Content must not exceed 1000 characters.")]
        public string Content { get; set; } = string.Empty;
    }
}