using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dto;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStock
    {
        private readonly AppDbContext _context;
        public StockRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckStock(int id)
        {
            return await _context.Stock.AnyAsync(s => s.Id == id);//AnyAsync() check if exist, if does'nt exist going to return null
        }

        public async Task<Stock> CrateAsync(Stock stockModel)
        {
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();

            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockmodel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if(stockmodel == null)
            {
                return null;
            }

             _context.Stock.Remove(stockmodel);
            await _context.SaveChangesAsync();

            return stockmodel;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stock.Include(c => c.Comments).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            var stock = await _context.Stock.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id == id);

            if(stock == null)
            {
                return null;
            }
            return stock;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var stockmodel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            
            if(stockmodel == null)
            {
                return null;
            }

            stockmodel.Industry = stockDto.Industry;
            stockmodel.Symbol = stockDto.Symbol;
            stockmodel.Purchase = stockDto.Purchase;
            stockmodel.MarketCap = stockDto.MarketCap;
            stockmodel.LastDiv = stockDto.LastDiv;
            stockmodel.CompanyName = stockDto.CompanyName;

            await _context.SaveChangesAsync();
            
            return stockmodel;
        }
    }
}