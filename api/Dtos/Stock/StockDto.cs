using api.Dtos.Comment;
using Newtonsoft.Json;

namespace api.Dtos.Stock
{
    public class StockDto : StockBaseDto
    {
        public int Id { get; set; }
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
        
    }
}