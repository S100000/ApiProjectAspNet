using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dto;
using api.Interfaces;
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
        private readonly IStock _stockRepo;
        public StockController(AppDbContext context, IStock stockRepo)
        {
            _stockRepo = stockRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if(!ModelState.IsValid)
                return BadRequest();
                
            var stock = await _stockRepo.GetAllAsync();
            var stockDto = stock.Select(s => s.ToStockDto());
            
            return Ok(stock);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest();
                
            var stock = await _stockRepo.GetByIdAsync(id);

            if(stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto stockDto)
        {
            if(!ModelState.IsValid)
                return BadRequest();
                
            var stockModel = await _stockRepo.CrateAsync(stockDto.ToStockFromCreateDto());
            return CreatedAtAction(nameof(GetById), new{ id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateById([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            if(!ModelState.IsValid)
                return BadRequest();
                
            var stockmodel = await _stockRepo.UpdateAsync(id, stockDto);
            
            if(stockmodel == null)
            {
                return NotFound();
            }

            return Ok(stockmodel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest();
                
            var stockmodel = await _stockRepo.DeleteAsync(id);
            if(stockmodel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
        
    }
}