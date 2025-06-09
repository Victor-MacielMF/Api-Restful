using api.Helpers;
using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync(QueryParameters queryParameters);

        Task<Comment?> GetByIdAsync(int id);

        Task<IdentityResult> CreateAsync(Comment comment);

        Task<IdentityResult> UpdateAsync(Comment comment);

        Task<IdentityResult> DeleteAsync(Comment comment);
    }
}