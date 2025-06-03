using api.Data;
using api.Interfaces.Repositories;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
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


        public async Task<IdentityResult> CreateAsync(Comment comment)
        {
            if (comment == null)
                return IdentityResult.Failed(new IdentityError { Description = "Comment cannot be null." });

            try
            {
                await _dbContext.Comments.AddAsync(comment);
                await _dbContext.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Error creating comment: {ex.Message}" });
            }
        }

        public async Task<IdentityResult> UpdateAsync(Comment comment)
        {
            if (comment == null)
                return IdentityResult.Failed(new IdentityError { Description = "Comment cannot be null." });

            Comment? existingComment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == comment.Id);
            if (existingComment == null)
                return IdentityResult.Failed(new IdentityError { Description = "Comment not found." });

            existingComment.Content = comment.Content;
            existingComment.StockId = comment.StockId;

            try
            {
                _dbContext.Comments.Update(existingComment);
                await _dbContext.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Error updating comment: {ex.Message}" });
            }
        }

        public async Task<IdentityResult> DeleteAsync(Comment comment)
        {
            if (comment == null)
                return IdentityResult.Failed(new IdentityError { Description = "Comment cannot be null." });

            try
            {
                _dbContext.Comments.Remove(comment);
                await _dbContext.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Error deleting comment: {ex.Message}" });
            }
        }
    }
}