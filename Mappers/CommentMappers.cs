using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        public static CommentDto ToCommentDto(this Comment comment)
        {
            if (comment == null)
            {
                return null;
            }

            return new CommentDto
            {
                CreatedBy = comment.Account.UserName,
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                StockId = comment.StockId,
            };
        }
        public static Comment ToCommentFromUpdateDTO(this UpdateCommentDto commentDto, Comment existingComment)
        {
            if (commentDto == null || existingComment == null)
            {
                return null;
            }

            existingComment.Title = commentDto.Title;
            existingComment.Content = commentDto.Content;

            return existingComment;
        }
        public static Comment ToCommentFromCreateDTO(this CreateCommentDto commentDto, int stockId, string accountId)
        {
            if (commentDto == null)
            {
                return null;
            }

            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId,
                AccountId = accountId
            };
        }
    }
}