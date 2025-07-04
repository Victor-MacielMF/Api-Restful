namespace api.Dtos.Comment
{
    public class CommentDto : CommentBaseDto
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? StockId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;

    }
}