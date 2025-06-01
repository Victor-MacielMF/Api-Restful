using api.Dtos.Comment;
using Newtonsoft.Json;

namespace api.Dtos.Stock
{
    public class StockDto : StockBaseDto
    {
        public int Id { get; set; }
        
        [JsonProperty(Order = 99)]
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}