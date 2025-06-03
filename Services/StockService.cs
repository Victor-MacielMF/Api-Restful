using api.Dtos;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;

namespace api.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;

        public StockService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<DataResponse<IEnumerable<StockWithoutCommentsDTO>>> GetStocks(QueryObject queryObject)
        {

            List<Stock> stocks = await _stockRepository.GetAllAsync(queryObject);
            if (stocks == null)
            {
                return new DataResponse<IEnumerable<StockWithoutCommentsDTO>>("No stocks found.");
            }
            IEnumerable<StockWithoutCommentsDTO> StocksDtos = stocks.Select(s => s.ToStockWithoutCommentsDto());

            return new DataResponse<IEnumerable<StockWithoutCommentsDTO>>("Stocks retrieved successfully.", StocksDtos);
        }
    }
}