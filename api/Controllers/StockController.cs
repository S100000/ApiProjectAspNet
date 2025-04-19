using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dto;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAll()
        {
            var stock = _context.Stock.ToList().Select(s => s.ToStockDto());
            return Ok(stock);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stock = _context.Stock.FirstOrDefault(x => x.Id == id);

            if(stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public IActionResult CreateStock([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            _context.Stock.Add(stockModel);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new{ id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateById([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            var stockmodel = _context.Stock.FirstOrDefault(x => x.Id == id);
            
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

            _context.SaveChanges();
            return Ok(stockmodel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stockmodel = _context.Stock.FirstOrDefault(x => x.Id == id);
            if(stockmodel == null)
            {
                return NotFound();
            }

            _context.Stock.Remove(stockmodel);
            _context.SaveChanges();

            return NoContent();
        }
        
    }
}