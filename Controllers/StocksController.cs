using api.Dtos;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        public StocksController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<IEnumerable<StockWithoutCommentsDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status404NotFound)]
        
        public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
        {
            var stocks = await _stockRepository.GetAllAsync(queryObject);
            if (stocks == null || !stocks.Any())
            {
                return NotFound(new MessageResponse("No stocks found."));
            }
            var StocksDtos = stocks.Select(s => s.ToStockWithoutCommentsDto());

            return Ok(new DataResponse<IEnumerable<StockWithoutCommentsDTO>>("Stocks retrieved successfully.", StocksDtos));
        }

        [HttpGet("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<StockDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound(new MessageResponse($"Stock with ID {id} not found."));
            }
            return Ok(new DataResponse<StockDto>("Stock retrieved successfully.", stock.TostockDto()));
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<StockWithoutCommentsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (stockDto == null)
            {
                return BadRequest(new MessageResponse("Stock data is null."));
            }
            var stock = stockDto.ToStockFromCreateDTO();
            if (stock == null)
            {
                return BadRequest(new MessageResponse("Invalid stock data."));
            }
            await _stockRepository.CreateAsync(stock);

            return Ok(new DataResponse<StockWithoutCommentsDTO>("Stock created successfully.", stock.ToStockWithoutCommentsDto()));
        }

        [HttpPut("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<StockDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            if (stockDto == null)
            {
                return BadRequest(new MessageResponse("Stock data is null."));
            }
            var existingStock = await _stockRepository.UpdateAsync(id, stockDto);
            if (existingStock == null)
            {
                return NotFound(new MessageResponse($"Stock with ID {id} not found."));
            }
            return Ok(new DataResponse<StockWithoutCommentsDTO>("Stock updated successfully.", existingStock.ToStockWithoutCommentsDto()));
        }


        [HttpDelete("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<StockDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await _stockRepository.DeleteAsync(id);
            if (stock == null)
            {
                return NotFound(new MessageResponse($"Stock with ID {id} not found."));
            }
            return Ok(new DataResponse<StockDto>("Stock deleted successfully.", stock.TostockDto()));
        }
    }
}