using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public CommentRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _dbContext.Comments.Include(c => c.Account).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _dbContext.Comments
                .Include(c => c.Stock)
                .Include(c => c.Account)
                .FirstOrDefaultAsync(c => c.Id == id);
        }


        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            var existingComment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == comment.Id);
            if (existingComment == null)
            {
                return null;
            }

            existingComment.Content = comment.Content;
            existingComment.StockId = comment.StockId;

            _dbContext.Comments.Update(existingComment);
            await _dbContext.SaveChangesAsync();
            return existingComment;
        }

        public async Task<Comment> DeleteAsync(int id)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
            {
                return null;
            }

            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();
            return comment;
        }
    }
}