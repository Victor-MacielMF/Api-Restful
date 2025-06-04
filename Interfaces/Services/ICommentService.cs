using api.Dtos;
using api.Dtos.Comment;

namespace api.Interfaces.Services
{
    public interface ICommentService
    {
        Task<DataResponse<IEnumerable<CommentDto>>> GetAllAsync();
        Task<DataResponse<CommentDto>> GetByIdAsync(int id);
        Task<DataResponse<CommentDto>> CreateAsync(int stockId, CreateCommentDto commentDto, string userId);
        Task<DataResponse<CommentDto>> UpdateAsync(int id, UpdateCommentDto commentDto, string userId);
        Task<DataResponse<CommentDto>> DeleteAsync(int id, string UserId);
    }
}