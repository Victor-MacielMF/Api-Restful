using api.Dtos;
using api.Dtos.Comment;

namespace api.Interfaces
{
    public interface ICommentService
    {
        Task<DataResponse<IEnumerable<CommentDto>>> GetAllCommentsAsync();
        Task<DataResponse<CommentDto>> GetCommentByIdAsync(int id);
        Task<DataResponse<CommentDto>> CreateCommentAsync(int stockId, CreateCommentDto commentDto, string userId);
        Task<DataResponse<CommentDto>> UpdateCommentAsync(int id, UpdateCommentDto commentDto);
        Task<DataResponse<CommentDto>> DeleteCommentAsync(int id);
    }
}