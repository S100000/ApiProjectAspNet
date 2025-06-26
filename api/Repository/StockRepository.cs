using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dto;
using api.Helpers;
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

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _context.Stock.Include(c => c.Comments).AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }
            if(!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }
            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescsending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
            }
            
            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
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