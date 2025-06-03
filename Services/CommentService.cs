using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Dtos.Comment;
using api.Interfaces.Repositories;
using api.Interfaces.Services;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;

        public CommentService(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }

        public async Task<DataResponse<IEnumerable<CommentDto>>> GetAllCommentsAsync()
        {
            List<Comment> comments = await _commentRepository.GetAllAsync();

            if (comments == null || !comments.Any())
                return new DataResponse<IEnumerable<CommentDto>>("No comments found.");

            IEnumerable<CommentDto> commentDtos = comments.Select(c => c.ToCommentDto());

            return new DataResponse<IEnumerable<CommentDto>>("Comments retrieved successfully.", commentDtos);
        }

        public async Task<DataResponse<CommentDto>> GetCommentByIdAsync(int id)
        {
            Comment? comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
                return new DataResponse<CommentDto>($"Comment with ID {id} not found.");

            return new DataResponse<CommentDto>("Comment retrieved successfully.", comment.ToCommentDto());
        }

        public async Task<DataResponse<CommentDto>> CreateCommentAsync(int stockId, CreateCommentDto commentDto, string userId)
        {
            if (commentDto == null)
                return new DataResponse<CommentDto>("Comment data is null.");

            bool stockExists = await _stockRepository.ExistsAsync(stockId);
            if (!stockExists)
                return new DataResponse<CommentDto>($"Stock with ID {stockId} not found.");

            Comment comment = commentDto.ToCommentFromCreateDTO(stockId, userId);
            if (comment == null)
                return new DataResponse<CommentDto>("Invalid comment data.");

            IdentityResult result = await _commentRepository.CreateAsync(comment);

            if (!result.Succeeded)
                return new DataResponse<CommentDto>(
                    result.Errors.FirstOrDefault()?.Description ?? "Failed to create comment."
                );

            // Busca o comentário criado para retornar o DTO completo
            Comment? userComment = await _commentRepository.GetByIdAsync(comment.Id);

            if (userComment == null)
                return new DataResponse<CommentDto>($"Created comment with ID {comment.Id} not found.");

            return new DataResponse<CommentDto>("Comment created successfully.", userComment.ToCommentDto());
        }

        public async Task<DataResponse<CommentDto>> UpdateCommentAsync(int id, UpdateCommentDto commentDto)
        {
            if (commentDto == null)
                return new DataResponse<CommentDto>("Comment data is null.");

            Comment? existingComment = await _commentRepository.GetByIdAsync(id);
            if (existingComment == null)
                return new DataResponse<CommentDto>($"Comment with ID {id} not found.");

            Comment updatedComment = commentDto.ToCommentFromUpdateDTO(existingComment);
            if (updatedComment == null)
                return new DataResponse<CommentDto>("Invalid comment data.");

            IdentityResult result = await _commentRepository.UpdateAsync(updatedComment);

            if (!result.Succeeded)
                return new DataResponse<CommentDto>(
                    result.Errors.FirstOrDefault()?.Description ?? "Failed to update comment."
                );

            // Buscar novamente para garantir o retorno atualizado
            Comment? refreshedComment = await _commentRepository.GetByIdAsync(id);

            return new DataResponse<CommentDto>("Comment updated successfully.", refreshedComment?.ToCommentDto());
        }

        public async Task<DataResponse<CommentDto>> DeleteCommentAsync(int id)
        {
            Comment? existingComment = await _commentRepository.GetByIdAsync(id);
            if (existingComment == null)
                return new DataResponse<CommentDto>($"Comment with ID {id} not found.");

            IdentityResult result = await _commentRepository.DeleteAsync(existingComment);

            if (!result.Succeeded)
                return new DataResponse<CommentDto>(
                    result.Errors.FirstOrDefault()?.Description ?? "Failed to delete comment."
                );

            return new DataResponse<CommentDto>("Comment deleted successfully.", existingComment.ToCommentDto());
        }
    }
}