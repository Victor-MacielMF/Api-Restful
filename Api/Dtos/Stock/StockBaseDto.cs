using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock
{
    public class StockBaseDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "Symbol must be at least 2 character long.")]
        [MaxLength(10, ErrorMessage = "Symbol must not exceed 10 characters.")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MinLength(2, ErrorMessage = "Company name must be at least 2 character long.")]
        [MaxLength(150, ErrorMessage = "Company name must not exceed 150 characters.")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Purchase price must be greater than 0.")]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Last dividend must be a non-negative value.")]
        public decimal LastDiv { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Industry must be at least 2 character long.")]
        [MaxLength(100, ErrorMessage = "Industry must not exceed 100 characters.")]
        public string Industry { get; set; } = string.Empty;

        [Required]
        [Range(0, long.MaxValue, ErrorMessage = "Market cap must be a non-negative value.")]
        public long MarketCap { get; set; }
    }
}