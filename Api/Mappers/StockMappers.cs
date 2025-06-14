using api.Dtos.Comment;
using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDto TostockDto(this Stock stock, string userId)
        {
            return new StockDto
            {
                Id = stock.Id,
                        Symbol = stock.Symbol,
                        CompanyName = stock.CompanyName,
                        Purchase = stock.Purchase,
                        LastDiv = stock.LastDiv,
                        Industry = stock.Industry,
                        MarketCap = stock.MarketCap,
                        Comments = stock.Comments
                            .Where(c => c.AccountId == userId)
                            .Select(c => c.ToCommentDto())
                            .ToList()
            };
        }

        public static StockDto TostockDto(this Stock stock)
        {
            return new StockDto
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap,
                Comments = stock.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }

        public static StockWithoutCommentsDTO ToStockWithoutCommentsDto(this Stock stock)
        {
            return new StockWithoutCommentsDTO
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap
            };
        }

        public static Stock ToStockFromCreateDTO(this CreateStockRequestDto stockDto)
        {
            return new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap
            };
        }
    }
}