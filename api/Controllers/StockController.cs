using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dto;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("Api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StockController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stock = await _context.Stock.ToListAsync();
            var stockDto = stock.Select(s => s.ToStockDto());
            
            return Ok(stock);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

            if(stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new{ id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            var stockmodel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            
            if(stockmodel == null)
            {
                return NotFound();
            }

            stockmodel.Industry = stockDto.Industry;
            stockmodel.Symbol = stockDto.Symbol;
            stockmodel.Purchase = stockDto.Purchase;
            stockmodel.MarketCap = stockDto.MarketCap;
            stockmodel.LastDiv = stockDto.LastDiv;
            stockmodel.CompanyName = stockDto.CompanyName;

            await _context.SaveChangesAsync();
            return Ok(stockmodel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockmodel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if(stockmodel == null)
            {
                return NotFound();
            }

             _context.Stock.Remove(stockmodel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
    }
}