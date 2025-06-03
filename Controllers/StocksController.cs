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
        private readonly IStockService _stockService;
        public StocksController(IStockRepository stockRepository, IStockService stockService)
        {
            _stockRepository = stockRepository;
            _stockService = stockService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<IEnumerable<StockWithoutCommentsDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
        {
            var response = await _stockService.GetStocks(queryObject);
            if (response.Errors != null)
            {
                BadRequest(response);
            }
            else if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<IEnumerable<StockDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            DataResponse<StockDto> response = await _stockService.GetStock(id);
            if (response.Errors != null)
            {
                BadRequest(response);
            }
            else if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<IEnumerable<StockWithoutCommentsDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            DataResponse<StockWithoutCommentsDTO> response = await _stockService.PostStock(stockDto);

            if (response.Errors != null)
            {
                BadRequest(response);
            }
            else if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
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
            return Ok(new DataResponse<StockWithoutCommentsDTO>("Stock updated successfully."));
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
            return Ok(new DataResponse<StockDto>("Stock deleted successfully."));
        }
    }
}