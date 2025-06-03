using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();

        Task<Comment?> GetByIdAsync(int id);

        Task<IdentityResult> CreateAsync(Comment comment);

        Task<IdentityResult> UpdateAsync(Comment comment);

        Task<IdentityResult> DeleteAsync(Comment comment);
    }
}