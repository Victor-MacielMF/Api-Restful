using api.Dtos;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces.Services;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [ApiController]
    [Route("api/v2/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;
        public StocksController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<IEnumerable<StockWithoutCommentsDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
        {
            var response = await _stockService.GetAllAsync(queryObject);
            if (response.Errors != null)
            {
                return BadRequest(response);
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
            DataResponse<StockDto> response = await _stockService.GetByIdAsync(id);
            if (response.Errors != null)
            {
                return BadRequest(response);
            }
            else if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<IEnumerable<StockWithoutCommentsDTO>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            DataResponse<StockWithoutCommentsDTO> response = await _stockService.CreateAsync(stockDto);

            if (response.Errors != null)
            {
                return BadRequest(response);
            }
            else if (response.Data == null)
            {
                return NotFound(response);
            }

            return CreatedAtAction(nameof(GetById), new { id = response.Data?.Id }, response);
        }

        [HttpPut("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<IEnumerable<StockWithoutCommentsDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            var response = await _stockService.UpdateAsync(id, stockDto);

            if (response.Errors != null)
            {
                return BadRequest(response);
            }
            else if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }


        [HttpDelete("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _stockService.DeleteAsync(id);
            if (response.Errors != null)
            {
                return BadRequest(response);
            }
            else if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}