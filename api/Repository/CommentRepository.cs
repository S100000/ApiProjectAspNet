using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dto.CommentsDto;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    
    public class CommentRepository : IComment
    {
        private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context)
        {
            _context = context;   
        }

        public async Task<Comments> CreateAsync(Comments commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<Comments> DeleteAsync(int id)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if(commentModel == null)
            {
                return null;
            }

            _context.Remove(commentModel);
            await _context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<List<Comments>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comments?> GetByIdAsync(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if(comment == null)
            {
                return null;
            }

            return comment;
        }

        public async Task<Comments> UpdateAsync(int id, UpdateCommentDto commentDto)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if(commentModel == null)
            {
                return null;
            }

            commentModel.Content = commentDto.Content;
            commentModel.Title = commentDto.Title;
            await _context.SaveChangesAsync();
            return commentModel;
        }
    }
}