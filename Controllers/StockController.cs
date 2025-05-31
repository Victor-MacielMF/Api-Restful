using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<StockDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
        {
            var stocks = await _stockRepository.GetAllAsync(queryObject);
            if (stocks == null || !stocks.Any())
            {
                return NotFound("No stocks found .");
            }
            var StockDtos = stocks.Select(s => s.TostockDto());

            return Ok(StockDtos);
        }

        [HttpGet("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StockDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound($"Stock with ID {id} not found.");
            }
            return Ok(stock.TostockDto());
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StockDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (stockDto == null)
            {
                return BadRequest("Stock data is null.");
            }
            var stock = stockDto.ToStockFromCreateDTO();
            if (stock == null)
            {
                return BadRequest("Invalid stock data.");
            }
            await _stockRepository.CreateAsync(stock);


            return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.TostockDto());
        }

        [HttpPut("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StockDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (stockDto == null)
            {
                return BadRequest("Stock data is null.");
            }

            var existingStock = await _stockRepository.UpdateAsync(id, stockDto);
            if (existingStock == null)
            {
                return NotFound($"Stock with ID {id} not found.");
            }

            return Ok(existingStock.TostockDto());
        }


        [HttpDelete("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StockDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await _stockRepository.DeleteAsync(id);
            if (stock == null)
            {
                return NotFound($"Stock with ID {id} not found.");
            }

            return Ok(stock.TostockDto());
        }
    }
}