
namespace api.Helpers
{
    public class StockQueryParameters : QueryParameters
    {
        public string? Symbol { get; set; } = null;
        public string? CompanyName { get; set; } = null;
        public string? SortBy { get; set; }
        public bool IsAscending { get; set; } = true;
    }
}